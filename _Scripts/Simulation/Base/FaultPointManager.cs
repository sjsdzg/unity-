using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Common;
using XFramework.Component;
using XFramework.Core;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.Simulation
{
    /// <summary>
    /// 故障点管理器
    /// </summary>
    public class FaultPointManager : Singleton<FaultPointManager>
    {
        private readonly string path = "Icons/Guide";

        /// <summary>
        /// 图标列表
        /// </summary>
        private Dictionary<string, Sprite> m_Textures = new Dictionary<string, Sprite>();

        /// <summary>
        /// 故障列表
        /// </summary>
        private Dictionary<string, FaultPointComponent> m_FaultPoints = new Dictionary<string, FaultPointComponent>();

        /// <summary>
        /// 故障信息
        /// </summary>
        public FaultInfo FaultInfo { get; set; }

        public class OnFindEvent : UnityEvent<string> { }

        private OnFindEvent m_OnFind = new OnFindEvent();
        /// <summary>
        /// 故障点发现事件
        /// </summary>
        public OnFindEvent OnFind
        {
            get { return m_OnFind; }
            set { m_OnFind = value; }
        }

        private UnityEvent n_OnFindAll = new UnityEvent();
        /// <summary>
        /// 发现所有故障点事件
        /// </summary>
        public UnityEvent OnFindAll
        {
            get { return n_OnFindAll; }
            set { n_OnFindAll = value; }
        }

        protected override void Init()
        {
            base.Init();
            Sprite[] array = Resources.LoadAll<Sprite>(path);
            for (int i = 0; i < array.Length; i++)
            {
                Sprite texture = array[i];
                m_Textures.Add(texture.name, texture);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="faultInfo"></param>
        public void Init(FaultInfo faultInfo, Transform parent)
        {
            FaultInfo = faultInfo;
            foreach (Transform child in parent)
            {
                FaultPointComponent faultPoint = child.GetOrAddComponent<FaultPointComponent>();
                faultPoint.hudInfo = new FaultPointInfo();
                faultPoint.hudInfo.onScreenArgs = new FaultPointOnScreenArgs();
                faultPoint.hudInfo.offScreenArgs = new FaultPointOffScreenArgs();
                faultPoint.hudInfo.HUDType = HUDType.FaultPoint;
                faultPoint.hudInfo.onScreenArgs.m_Sprite = m_Textures["Aim-Border-Icon"];
                faultPoint.hudInfo.onScreenArgs.m_Color = Color.red;
                faultPoint.hudInfo.onScreenArgs.flashing = true;
                faultPoint.hudInfo.offScreenArgs.m_Sprite = m_Textures["Arrow"];
                faultPoint.hudInfo.offScreenArgs.m_Color = Color.red;
                faultPoint.hudInfo.m_Target = child;
                faultPoint.OnFind.AddListener(faultPoint_OnFind);
                m_FaultPoints.Add(faultPoint.name, faultPoint);
            }
            //显示故障点
            ShowFaultPoints();
        }

        public void ShowFaultPoints()
        {
            foreach (string name in m_FaultPoints.Keys)
            {
                m_FaultPoints[name].hudInfo.onScreenArgs.m_Text = FaultInfo.GetFaultPhenomena(name).Phenomena;
                m_FaultPoints[name].show();

            }
        }

        /// <summary>
        /// 发现数量
        /// </summary>
        private int count = 0;

        /// <summary>
        /// 故障点发现时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void faultPoint_OnFind(string name)
        {
            OnFind.Invoke(name);
            EventDispatcher.ExecuteEvent<string>(Events.Fault.AddPhenomena, name);

            if (count < FaultInfo.FaultPhenomenas.Count)
            {
                count++;
            }

            if (count == FaultInfo.FaultPhenomenas.Count)
            {
                OnFindAll.Invoke();
            }
        }

        /// <summary>
        /// 主动发现故障点
        /// </summary>
        /// <param name="name"></param>
        public void Find(string name)
        {
            faultPoint_OnFind(name);
        }

        public FocusComponent GetFocusComponent(string name)
        {
            FaultPointComponent faultPoint = null;
            if (m_FaultPoints.TryGetValue(name, out faultPoint))
            {
                return faultPoint.GetComponentInChildren<FocusComponent>();
            }
            else
            {
                return null;
            }
        }
    }
}
