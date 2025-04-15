
using Simulation.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Component;
using XFramework.Core;
using XFramework.Module;
using XFramework.Simulation;
namespace XFramework.PLC
{
    /// <summary>
    /// PLC 阀门 流体管理类
    /// </summary>
    public class PLCPipeFittingManager : MonoBehaviour
    {

        public UniEvent<string> ValveAfterEvent = new UniEvent<string>();
        /// <summary>
        /// 当前流程节点
        /// </summary>
        public string CurrFlowId { get; set; }

        /// <summary>
        /// 管件
        /// </summary>
        private PipeFitting m_PipeFitting;

        /// <summary>
        /// PLC阀门组件字典
        /// </summary>
        private Dictionary<string, PLCValveComponent> m_ValveComponents = new Dictionary<string, PLCValveComponent>();

        /// <summary>
        /// uniqueValveNames
        /// </summary>
        private List<string> uniqueValveNames = new List<string>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="valveParent"></param>
        public void Init(string configPath, Transform valveParent)
        {
            uniqueValveNames.Clear();
            m_ValveComponents.Clear();
            m_PipeFitting = PipeFitting.Parser.ParseXmlFromResources(configPath);
            if (m_PipeFitting != null)
            {
                foreach (FittingNode node in m_PipeFitting.FittingNodes)
                {
                    //uniqueValveNames
                    foreach (Valve valve in node.Valves)
                    {
                        if (!uniqueValveNames.Contains(valve.Name))
                        {
                            if (ValveManager.Instance.GetValveName2D(valve.Name) != null)
                            {
                                if (!uniqueValveNames.Contains(ValveManager.Instance.GetValveName2D(valve.Name)))
                                {
                                    uniqueValveNames.Add(ValveManager.Instance.GetValveName2D(valve.Name));
                                }
                            };
                        }
                    }
                }
                //Loading
                CoroutineManager.Instance.StartCoroutine(Loading(valveParent));
            }
            else
            {
                Debug.LogError("管道流体配置表路径不存在或配置表为空！");
            }

        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="pipeParent"></param>
        /// <returns></returns>
        IEnumerator Loading(Transform valveParent)
        {
            yield return new WaitForEndOfFrame();
            foreach (string valveName in uniqueValveNames)
            {
                Transform child = valveParent.Find(valveName);
                if (child != null)
                {
                    PLCValveComponent component = child.GetOrAddComponent<PLCValveComponent>();
                    m_ValveComponents.Add(child.name, component);
                }
            }
        }

        /// <summary>
        /// 调用当前节点阀门和流体
        /// </summary>
        //public void InvokeCurrentFitting()
        //{
        //    string name = string.Format("{0}-{1}", CurrFlowId);
        //    InvokeFitting(name);
        //}

        /// <summary>
        /// 调用当前节点阀门和流体
        /// </summary>
        public void InvokeCurrentFitting(int number)
        {
            string name = string.Format("{0}-{1}", CurrFlowId, number);

            InvokeFitting(name);
            //调用当前 流体 
            PipeFittingManager.Instance.InvokeFitting(name);
        }
        private int ValveCloseCount = 0;
        FittingNode node;
        string nodeName;
        private List<PLCValveComponent> pLCValveComponents = new List<PLCValveComponent>();
        /// <summary>
        /// 根据节点名称，调用节点
        /// </summary>
        /// <param name="_nodeName"></param>
        public void InvokeFitting(string _nodeName,bool isShaking=true, PLCValveComponent.Shake shake = PLCValveComponent.Shake.Flickering)
        {
            node = m_PipeFitting.FittingNodes.Find(x => x.Name == _nodeName);
            if (node != null)
            {
                nodeName = _nodeName;
                //Valve
                foreach (Valve valve in node.Valves)
                {
                    PLCValveComponent component = null;
                    if (m_ValveComponents.TryGetValue(ValveManager.Instance.GetValveName2D(valve.Name), out component))
                    {
                        if (!pLCValveComponents.Contains(component))
                        {
                            pLCValveComponents.Add(component);
                            component.OnClick.AddListener(Valve_onClick);
                        }                                            
                        component.State = valve.State;
                        component.SetReadyState(isShaking, shake);
                    }
                }
            }

        }

        public void CloseAllFitting()
        {
            foreach (PLCValveComponent item in pLCValveComponents)
            {
                item.OnClick.RemoveListener(Valve_onClick);
                item.State = ValveState.OFF;
                item.SetReadyState(false);
            }
            pLCValveComponents.Clear();
            ValveCloseCount = 0;
        }

        private void Valve_onClick()
        {
            ValveCloseCount++;
            Debug.Log("阀门打开的次数 = " + ValveCloseCount+ "  nodeName=  " + nodeName);
            if (ValveCloseCount == node.Valves.Count)
            {
                ValveCloseCount = 0;
                ValveAfterEvent.Invoke(nodeName);
            }
            Debug.Log("当前阀门打开的次数 = " + ValveCloseCount );
        }
    }
}
