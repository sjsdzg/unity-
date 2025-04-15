using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Simulation.Component
{
    /// <summary>
    /// 判断内部外部组件
    /// </summary>
	public class InterOrExter : MonoBehaviour {


        /// <summary>
        /// 容器名字
        /// </summary>
        public string Name = "";
  
        [SerializeField]
        private bool isInter = false;
        /// <summary>
        /// 是否在里面
        /// </summary>
        public bool IsInter
        {
            get { return isInter; }
            set { 
                if(isInter==value)
                {
                    return;
                }
                isInter = value;
                RoomTriggerArg arg = new RoomTriggerArg(Name,isInter);
                OnInterOrExterEvent.Invoke(gameObject,arg);

            }
        }


        public RoomTriggerComponent[] triggerCom = new RoomTriggerComponent[2];
        /// <summary>
        /// 进入出去事件
        /// </summary>
        public class InterOrExterEvent : UnityEvent<GameObject,RoomTriggerArg> {}

        private InterOrExterEvent m_InterOrExterEvent = new InterOrExterEvent();

 
        public InterOrExterEvent OnInterOrExterEvent
        {
            get { return m_InterOrExterEvent; }
            set { m_InterOrExterEvent = value; }
        }

        // Use this for initialization
        void Start () {
            if(Name== string.Empty)
            {
                Name = name;
            }
            for (int i = 0; i < triggerCom.Length; i++)
            {
                if (triggerCom[i].spaceType == SpaceType.Inter)
                {
                    triggerCom[i].onTouchEvent.AddListener((sender, arg) => { IsInter = true;  arg.CallBack();});
                }
                else if (triggerCom[i].spaceType == SpaceType.Exter)
                {
                    triggerCom[i].onTouchEvent.AddListener((sender, arg) => { IsInter = false; arg.CallBack();});
                }
            }
		}
	}


    /// <summary>
    /// 房间触发参数
    /// </summary>
    public class RoomTriggerArg
    {
        /// <summary>
        /// 触发房间
        /// </summary>
        public string  Name{ get; set; }

        /// <summary>
        /// 是否进入
        /// </summary>
        public bool IsInter { get; set; }

        /// <summary>
        /// 进入时间
        /// </summary>
        public DateTime InterTime { get; set; }


        public RoomTriggerArg(string _name,bool _isInter)
        {
            Name = _name;

            IsInter = _isInter;

            InterTime = DateTime.Now;
        }
    }
}