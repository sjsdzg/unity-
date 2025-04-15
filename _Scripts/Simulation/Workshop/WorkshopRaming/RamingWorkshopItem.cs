using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Simulation.Component
{
    /// <summary>
    /// 漫游房间
    /// </summary>
	public class RamingWorkshopItem : MonoBehaviour
    {

        /// <summary>
        /// 房间名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 房间进入退出触发器
        /// </summary>
        //private InterOrExter m_InterOrExter;


        /// <summary>
        /// 房间轮廓
        /// </summary>
        private WorkshopOutline m_WorkshopOutline;

        /// <summary>
        /// 房间最佳视角
        /// </summary>
        private BestAngle m_BestAngle;

        /// <summary>
        /// 房间标签
        /// </summary>
        private LabelItem m_LabelItem;

        /// <summary>
        /// 人物房间初始位置
        /// </summary>
        public Transform InitialPosition;

        public class WorkshopClickEvent : UnityEvent<string> { }

        private WorkshopClickEvent m_OnClickLabel = new WorkshopClickEvent();

        public WorkshopClickEvent OnClickLabel
        {
            get { return m_OnClickLabel; }
            set { m_OnClickLabel = value; }
        }

        private void Awake()
        {
            Name = name;
            InitGameObject();
            InitEvent();
        }

        void InitGameObject()
        {
            //m_WorkshopOutline = transform.Find("房间轮廓").GetComponent<WorkshopOutline>();
            m_BestAngle = transform.Find("BestAngle").GetComponent<BestAngle>();
            m_LabelItem = transform.Find("Canvas/Icon").GetComponent<LabelItem>();
            InitialPosition = transform.Find("初始位置");
        }

        void InitEvent()
        {
            //m_WorkshopOutline.OnClickWorkShop.AddListener(workshopOutline_OnClickWorkShop);

            m_LabelItem.gameObject.GetComponent<Button>().onClick.AddListener(LabelItem_OnClick);
        }

        /// <summary>
        /// 标签按钮点击
        /// </summary>
        private void LabelItem_OnClick()
        {
            OnClickLabel.Invoke(Name);

            m_BestAngle.Enter();
        }

        /// <summary>
        /// 点击房间
        /// </summary>
        private void workshopOutline_OnClickWorkShop()
        {
            //m_WorkshopOutline.ShowOutLine();
            m_BestAngle.Enter();
            m_WorkshopOutline.IsAffect = false;
        }

	}
}