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
    /// 房间进入 出去管理器
    /// </summary>
	public class RoomTrigggerController : MonoBehaviour {

        /// <summary>
        /// 房间进出触发器
        /// </summary>
        public List<InterOrExter> InterList = new List<InterOrExter>();
        /// <summary>
        /// 进入出去事件
        /// </summary>
        public class InterOrExterEvent : UnityEvent<UnityEngine.GameObject, RoomTriggerArg> { }

        private InterOrExterEvent m_InterOrExterEvent = new InterOrExterEvent();


        public InterOrExterEvent OnInterOrExterEvent
        {
            get { return m_InterOrExterEvent; }
            set { m_InterOrExterEvent = value; }
        }
        // Use this for initialization
        void Start () {
            for (int i = 0; i < transform.childCount; i++)
            {
                InterOrExter temp = transform.GetChild(i).GetComponent<InterOrExter>();
                InterList.Add(temp);
                temp.OnInterOrExterEvent.AddListener(ReceiveRoomInfo);
            }
		}

        /// <summary>
        /// 设置房间状态
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_bo"></param>
        public void SetRoomState(string _name,bool _bo)
        {
            InterList.ForEach((x)=> {
                if(x.name == _name)
                {
                    x.IsInter = _bo;
                }

            });
        }
        /// <summary>
        /// 接受房间信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
		private void ReceiveRoomInfo(UnityEngine.GameObject sender,RoomTriggerArg arg)
        {
            OnInterOrExterEvent.Invoke(sender,arg);
        }
	}
}