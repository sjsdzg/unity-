using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Simulation;

namespace XFramework.UI
{
    /// <summary>
    /// 沙盘
    /// </summary>
    public class SandTable : MonoBehaviour
    {
        /// <summary>
        /// 当前的流程索引
        /// </summary>
        private int currentIndex =0;

        /// <summary>
        /// 灯光
        /// </summary>
        private Transform m_Light;

        private List<StageFlowComponent> m_ProcessFlowList = new List<StageFlowComponent>();
        /// <summary>
        /// 工艺流程
        /// </summary>
        public List<StageFlowComponent> ProcessFlowList
        {
            get { return m_ProcessFlowList; }
            set { m_ProcessFlowList = value; }
        }

        public int MyProperty { get; set; }

        /// <summary>
        /// 最佳视角
        /// </summary>
        private FocusComponent m_FocusComponent;

        public class OnClickedEvent : UnityEvent<string> { }

        private OnClickedEvent m_StagePointClicked = new OnClickedEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public OnClickedEvent StagePointClicked
        {
            get { return m_StagePointClicked; }
            set { m_StagePointClicked = value; }
        }

        void Awake()
        {
            Transform objProcess = transform.Find("工艺流程");
            for (int i = 0; i < objProcess.childCount; i++)
            {
                StageFlowComponent com = objProcess.Find("工艺流程 (" + (i+1) + ")").GetComponent<StageFlowComponent>();
                //点击事件
                com.StagePointClicked.AddListener(x => { StagePointClicked.Invoke(x); });
                m_ProcessFlowList.Add(com);
            }

            m_FocusComponent = transform.Find("Focus").GetComponent<FocusComponent>();
            m_Light = transform.Find("Light");
     
        }

        /// <summary>
        /// 沙盘展示
        /// </summary>
        public void Display()
        {
            gameObject.SetActive(true);
            m_FocusComponent.Focus();
        }

        /// <summary>
        /// 关闭沙盘
        /// </summary>
        public void Close()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 流程展示
        /// </summary>
        /// <param name="index"></param>
        public void DisplayProcess(int index)
        {
            //for (int i = 0; i < m_ProcessFlowList.Count; i++)
            //{
            //    m_ProcessFlowList[i].Disappear();
            //}
            m_ProcessFlowList[index].Appear();
            currentIndex = index;
        }
    }
}
