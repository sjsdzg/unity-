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
    /// 漫游模块 沙盘控制
    /// </summary>
	public class WorkSandTableController : MonoBehaviour {


        /// <summary>
        /// 沙盘字典
        /// </summary>
        private Dictionary<string,WorkSandTable> m_SandTables = null;

        private string currentName = null;
        /// <summary>
        /// 当前展示的沙盘
        /// </summary>
        public string CurrentName
        {
            get { return currentName; }
            private set { currentName = value; }
        }

        private int currentIndex = 0;

        /// <summary>
        /// 当前流程 索引
        /// </summary>
        public int CurrentIndex
        {
            get { return currentIndex; }
            set { currentIndex = value; }
        }



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

        void Awake()
        {
            m_SandTables = new Dictionary<string, WorkSandTable>();

            for (int i = 0; i < transform.childCount; i++)
            {
                string name = (i + 1).ToString();
                WorkSandTable table = transform.Find(name).GetComponent<WorkSandTable>();
                table.WorkPointClicked.AddListener(x => { WorkPointClicked.Invoke(x); });
                m_SandTables.Add(table.name, table);
            }
        }

        /// <summary>
        /// 根据name，显示沙盘
        /// </summary>
        /// <param name="name"></param>
        public void DisplaySandTable(string name)
        {
            if (!m_SandTables.ContainsKey(name))
                return;

            foreach (var key in m_SandTables.Keys)
            {
                if (key == name)
                {
                    m_SandTables[name].Display();
                }
                else
                {
                    m_SandTables[key].Close();
                }
            }

            currentName = name;
        }


        /// <summary>
        /// 展示当前工艺展示
        /// </summary>
        public void DisplayCurrentProcess()
        {
            ///获取指定沙盘
            WorkSandTable st = m_SandTables[currentName];

            ///打开指定流程
            st.ProcessDemo(CurrentIndex);
        }

        /// <summary>
        /// 分发消息 根据房间名字 选中房间高亮
        /// </summary>
        public void SetHighlightByName(string name)
        {
            ///获取指定沙盘
            WorkSandTable st = m_SandTables[currentName];
            st.SetHighlightByName(name);
        }

        public void SetBuildingTransparency()
        {
            ///获取指定沙盘
            WorkSandTable st = m_SandTables[currentName];
            st.SetBuildingTransparency();
        }
        public void ResetBuildingTransparency()
        {
            ///获取指定沙盘
            WorkSandTable st = m_SandTables[currentName];
            st.ResetBuildingInfo();
        }
    }
}