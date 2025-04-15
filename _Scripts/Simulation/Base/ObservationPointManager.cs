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
    public class ObservationPointManager:Singleton<ObservationPointManager>
    {
        private readonly string path = "Icons/Guide";

        /// <summary>
        /// 图标列表
        /// </summary>
        private Dictionary<string, Sprite> m_Textures = new Dictionary<string, Sprite>();

        /// <summary>
        /// 故障列表
        /// </summary>
        private Dictionary<string, ObservationPointCompontent> m_ObservationPoints = new Dictionary<string, ObservationPointCompontent>();

        /// <summary>
        /// 观察点信息
        /// </summary>
        public ObservationPointInfo ObservationPointInfo { get; set; }

        public class OnFindEvent : UnityEvent<string> { }

        private OnFindEvent m_OnFind = new OnFindEvent();
        /// <summary>
        /// 观察点点发现事件
        /// </summary>
        public OnFindEvent OnFind
        {
            get { return m_OnFind; }
            set { m_OnFind = value; }
        }

        private UnityEvent n_OnFindAll = new UnityEvent();
        /// <summary>
        /// 发现所有观察点事件
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
        public void Init( Transform parent)
        {
            foreach (Transform child in parent)
            {
                ObservationPointCompontent observationPoint = child.GetOrAddComponent<ObservationPointCompontent>();
                observationPoint.hudInfo = new ObservationPointInfo();
                observationPoint.hudInfo.onScreenArgs = new ObservationPointOnScreenArgs();
                observationPoint.hudInfo.offScreenArgs = new ObservationPointOffScreenArgs();
                observationPoint.hudInfo.HUDType = HUDType.ObservationPoint;
                observationPoint.hudInfo.onScreenArgs.m_Sprite = m_Textures["Eye-Icon"];
                observationPoint.hudInfo.onScreenArgs.m_Color = Color.white;
                observationPoint.hudInfo.onScreenArgs.flashing = true;
                observationPoint.hudInfo.offScreenArgs.m_Sprite = m_Textures["Arrow"];
                observationPoint.hudInfo.offScreenArgs.m_Color = Color.red;
                observationPoint.hudInfo.offScreenArgs.visible =false;
                observationPoint.hudInfo.m_Target = child;
                observationPoint.OnFind.AddListener(ObservationPoint_OnFind);
                m_ObservationPoints.Add(observationPoint.name, observationPoint);
            }
            //显示观察点
            ShowObservationPoints();
        }

        public void ShowObservationPoints()
        {
            foreach (string name in m_ObservationPoints.Keys)
            {
                m_ObservationPoints[name].hudInfo.onScreenArgs.m_Text = "拉进视角";
                m_ObservationPoints[name].show();

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
        private void ObservationPoint_OnFind(string name)
        {
            OnFind.Invoke(name);
            //EventDispatcher.ExecuteEvent<string>(Events.Fault.AddPhenomena, name);
            //EventDispatcher.ExecuteEvent<string>(Events.Observation.AddPhenomena, name);
            //if (count < FaultInfo.FaultPhenomenas.Count)
            //{
            //    count++;
            //}

            //if (count == FaultInfo.FaultPhenomenas.Count)
            //{
            //    OnFindAll.Invoke();
            //}
        }

        /// <summary>
        /// 主动发现故障点
        /// </summary>
        /// <param name="name"></param>
        public void Find(string name)
        {
            //faultPoint_OnFind(name);
        }

        public FocusComponent GetFocusComponent(string name)
        {
            ObservationPointCompontent observationPoint= null;
            if (m_ObservationPoints.TryGetValue(name, out observationPoint))
            {
                return observationPoint.GetComponentInChildren<FocusComponent>();
            }
            else
            {
                return null;
            }
        }


    }
}
