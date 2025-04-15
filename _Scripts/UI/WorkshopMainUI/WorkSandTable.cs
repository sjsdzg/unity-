using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 漫游模块 沙盘
    /// </summary>
	public class WorkSandTable : MonoBehaviour {

        /// <summary>
        /// 当前的流程索引
        /// </summary>
        public int currentIndex = 0;


        /// <summary>
        /// 工艺流程
        /// </summary>
        private List<WorkTagFlow> m_ProcessFlowList = new List<WorkTagFlow>();

        /// <summary>
        /// 房间轮廓组件
        /// </summary>
        private WorkOutLineComponent outLineCom;

        /// <summary>
        /// 沙盘中的土建的透明度设置
        /// </summary>
        private BuildingTransparency buildingTran;
        //private List<> 
        /// <summary>
        /// 最佳视角
        /// </summary>
        private BestAngle m_BestAngle;

        public class OnClickedEvent : UnityEvent<string> { }

        private OnClickedEvent m_WorkPointClicked = new OnClickedEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public OnClickedEvent WorkPointClicked
        {
            get { return m_WorkPointClicked; }
            set { m_WorkPointClicked = value; }
        }

        private void Awake()
        {
            Transform objProcess = transform.Find("工艺流程");
            for (int i = 0; i < objProcess.childCount; i++)
            {
                WorkTagFlow com = objProcess.Find("工艺流程 (" + (i + 1) + ")").GetComponent<WorkTagFlow>();
                //点击事件
                com.WorkPointClicked.AddListener(x => { WorkPointClicked.Invoke(x); });
                m_ProcessFlowList.Add(com);
            }

            m_BestAngle = transform.Find("BestAngle").GetComponent<BestAngle>();
            outLineCom = transform.Find("房间轮廓").GetComponent<WorkOutLineComponent>();
            buildingTran = transform.Find("土建").GetComponent<BuildingTransparency>();
        }
        /// <summary>
        /// 沙盘展示
        /// </summary>
        public void Display()
        {
            gameObject.SetActive(true);
            m_BestAngle.Enter();
            ResetBuildingInfo();
            HiteHighlight();
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
        public void ProcessDemo(int index)
        {
            for (int i = 0; i < m_ProcessFlowList.Count; i++)
            {
                m_ProcessFlowList[i].Disappear();
            }

            m_ProcessFlowList[index].Appear();
            currentIndex = index;
        }
        /// <summary>
        /// 显示高亮
        /// </summary>
        /// <param name="name"></param>
        public void SetHighlightByName(string name)
        {
            outLineCom.ShowHighlight(name);
        }
        public void HiteHighlight()
        {
            outLineCom.HideHighlight();
        }

        /// <summary>
        /// 设置土建透明
        /// </summary>
        public void SetBuildingTransparency()
        {
            buildingTran.SetTransparency();

        }
        public void ResetBuildingInfo()
        {
            buildingTran.ResetInfo();
        }
    }
}