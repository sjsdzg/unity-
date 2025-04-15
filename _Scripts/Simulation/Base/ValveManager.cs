using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Core;
using XFramework.Module;
using System.Text;
using XFramework.Component;
using XFramework.Core;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using XFramework.Common;

namespace XFramework.Simulation
{
    /// <summary>
    /// 管件管理器 (阀门 管道 流体)
    /// </summary>
    public class ValveManager : Singleton<ValveManager>
    {
        public class OnClickedEvent : UnityEvent<GameObject, string> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 点击触发
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// 阀门配置字典
        /// </summary>
        private Dictionary<string, Valve> m_Valves = new Dictionary<string, Valve>();

        /// <summary>
        /// 阀门组件字典
        /// </summary>
        public Dictionary<string, ValveComponent> m_ValveComponents = new Dictionary<string, ValveComponent>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configPath">阀门配置表路径</param>
        /// <param name="parent">阀门父节点</param>
        public void Init(string configPath, Transform parent)
        {
            m_Valves.Clear();
            m_ValveComponents.Clear();

            ValveCollection m_Collection = ValveCollection.Parser.ParseXmlFromResources(configPath);
            if (m_Collection != null)
            {
                foreach (var valve in m_Collection.Valves)
                {
                    m_Valves.Add(valve.Name, valve);
                }
                CoroutineManager.Instance.StartCoroutine(Loading(parent));
            }
            else
            {
                Debug.LogError("阀门配置表路径不存在或配置表为空！");
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configPath">阀门配置表路径</param>
        /// <param name="parent">阀门父节点</param>
        public void Init(string configPath)
        {
            m_Valves.Clear();
            m_ValveComponents.Clear();

            ValveCollection m_Collection = ValveCollection.Parser.ParseXmlFromResources(configPath);
            if (m_Collection != null)
            {
                foreach (var valve in m_Collection.Valves)
                {
                    m_Valves.Add(valve.Name, valve);
                }
            }
            else
            {
                Debug.LogError("阀门配置表路径不存在或配置表为空！");
            }
        }
        public string GetValveName2D(string name)
        {
            if (m_Valves.Count != 0)
            {
                Valve valve = null;
                if (m_Valves.TryGetValue(name, out valve))
                {
                    return valve.Name2D;
                }
            }
            return null;
        }
        IEnumerator Loading(Transform parent)
        {
            yield return new WaitForEndOfFrame();
            foreach (Transform child in parent)
            {
                Valve valve = null;
                if (m_Valves.TryGetValue(child.name, out valve))
                {
                    Transform temp = null;
                    if (child.name.Contains("VH") || child.name.Contains("VA"))//止回阀 安全阀 气动截止阀 气动球阀
                    {
                        temp = child.FuzzyFind("body");
                    }
                   else if (child.childCount==0)
                    {
                        temp = child;
                    }
                    else
                    {
                        temp = child.FuzzyFind("handle");
                    }

                    if (temp != null)
                    {
                        try
                        {
                            ValveComponent component = temp.GetOrAddComponent<ValveComponent>();
                            component.UUID = valve.Name;
                            if (component.UUID.Contains("VJ") || component.UUID.Contains("GD"))
                            {
                                component.angle = new Vector3(0, 0, 360);
                                component.duration = 2f;
                            }
                            else if (component.UUID.Contains("VQ"))
                            {
                                component.angle = new Vector3(0, 0, -90);
                                component.duration = 1f;
                            }
                            else
                            {
                                component.angle = Vector3.zero;
                            }
                            component.State = valve.State;
                            m_ValveComponents.Add(valve.Name, component);
                            component.TriggerAction(EventTriggerType.PointerClick, x => {
                                PointerEventData eventData = x as PointerEventData;
                                //左键点击
                                if (eventData.button == PointerEventData.InputButton.Left)
                                {
                                    OnClicked.Invoke(component.gameObject, component.UUID);
                                }
                            });
                            component.TriggerAction(EventTriggerType.PointerEnter, x => valveComponent_OnPointerEnter(component));
                            component.TriggerAction(EventTriggerType.PointerExit, x => valveComponent_OnPointerExit(component));
                        }
                        catch (Exception)
                        {
                            Debug.LogErrorFormat("阀门：{0} 重复", valve.Name);
                        }
                        
                    }
                }
            }
        }

        /// <summary>
        /// OnPointerEnter
        /// </summary>
        /// <param name="component"></param>
        private void valveComponent_OnPointerEnter(ValveComponent component)
        {
            Valve valve = null;
            StringBuilder sb = new StringBuilder();
            if (m_Valves.TryGetValue(component.UUID, out valve))
            {
                sb.Append("阀门名称：" + valve.ChineseName + "\n");
                sb.Append("所属设备：" + valve.Equipment + "\n");
                sb.Append("阀门种类：" + ValveDefine.GetValveName(valve.Type) + "\n");
                sb.Append("工作状态：" + component.State.ToString() + "\n");
                sb.Append("公称直径：" + valve.NominalDiameter + "\n");
                sb.Append("通过介质：" + valve.Medium);
                //显示高亮和提示
                Task.NewTask()
                    .Append(new HighlighterAction(component.gameObject, true))
                    .Append(new TooltipAction(true, sb.ToString()))
                    .Execute();
            }
        }

        /// <summary>
        /// OnPointerExit
        /// </summary>
        /// <param name="component"></param>
        private void valveComponent_OnPointerExit(ValveComponent component)
        {
            //显示高亮和提示
            Task.NewTask()
                .Append(new HighlighterAction(component.gameObject, false))
                .Append(new TooltipAction(false))
                .Execute();
        }

        /// <summary>
        /// 获取阀门组件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetValve(string name)
        {
            ValveComponent component = null;
            m_ValveComponents.TryGetValue(name, out component);
            return component.gameObject;
        }
    }
}
