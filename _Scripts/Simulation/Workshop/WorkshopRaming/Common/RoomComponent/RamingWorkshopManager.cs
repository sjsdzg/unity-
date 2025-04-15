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
	public class RamingWorkshopManager : MonoBehaviour {

        /// <summary>
        /// 房间进出触发器
        /// </summary>
        public List<InterOrExter> InterList = new List<InterOrExter>();

        /// <summary>
        /// 房间个体触发器
        /// </summary>
        public List<RamingWorkshopItem> roomList = new List<RamingWorkshopItem>();
        /// <summary>
        /// 漫游房间字典
        /// </summary>
        public Dictionary<string, GameObject> RamingRoomDic = new Dictionary<string, GameObject>();

        /// <summary>
        /// 所呆的房间
        /// </summary>
        private GameObject StayTheRoom;
        public class InterOrExterEvent : UnityEvent<GameObject, RoomTriggerArg> { }


        private InterOrExterEvent m_InterOrExterEvent = new InterOrExterEvent();

        /// <summary>
        /// 进入出去事件
        /// </summary>
        public InterOrExterEvent OnInterOrExterEvent
        {
            get { return m_InterOrExterEvent; }
            set { m_InterOrExterEvent = value; }
        }

        public class WorkshopClickEvent : UnityEvent<string> { }
        private WorkshopClickEvent m_OnClickWorkshop = new WorkshopClickEvent();

        /// <summary>
        /// 房间点击事件
        /// </summary>
        public WorkshopClickEvent OnClickWorkshop
        {
            get { return m_OnClickWorkshop; }
            set { m_OnClickWorkshop = value; }
        }

        // Use this for initialization
        void Start () {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject obj = transform.GetChild(i).gameObject;
                Ragister(obj.name,obj);
                InterOrExter temp = obj.GetComponent<InterOrExter>();
                if (temp == null)
                    continue;
                InterList.Add(temp);
                temp.OnInterOrExterEvent.AddListener(ReceiveRoomInfo);

                RamingWorkshopItem rItem = obj.GetComponent<RamingWorkshopItem>();
                if(rItem!=null)
                {
                    roomList.Add(rItem);
                    rItem.OnClickLabel.AddListener(ReceiveRoomClickInfo);
                }
            }
		}
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        public void Ragister(string name,GameObject obj)
        {
            if (!RamingRoomDic.ContainsKey(name))
            {
                RamingRoomDic.Add(name,obj);
            }

        }
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetRoomObject(string name)
        {
            GameObject obj = null;
            if(RamingRoomDic.TryGetValue(name.Trim(),out obj))
            {
                return obj;
            }
            return null;
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
		private void ReceiveRoomInfo(GameObject sender,RoomTriggerArg arg)
        {
            OnInterOrExterEvent.Invoke(sender,arg);
            if (arg.IsInter)
            {
                StayTheRoom = GetRoomObject(arg.Name);
            }
            else
            {
                StayTheRoom = null;
            }
        }

        /// <summary>
        /// 设置房间信息状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        public void SetRoomInfo(GameObject sender,RoomTriggerArg arg)
        {
            OnInterOrExterEvent.Invoke(sender, arg);
            if (arg.IsInter)
            {
                StayTheRoom = GetRoomObject(arg.Name);
            }
            else
            {
                StayTheRoom = null;
            }
        }
        /// <summary>
        /// 房间点击
        /// </summary>
        /// <param name="name"></param>
        private void ReceiveRoomClickInfo(string name)
        {
            OnClickWorkshop.Invoke(name);

        }
        /// <summary>
        /// 返回所处的房间
        /// </summary>
        /// <returns></returns>
        public GameObject GetStayTheRoom()
        {
            return StayTheRoom;

        }
	}
}