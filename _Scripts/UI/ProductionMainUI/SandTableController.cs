using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 沙盘控制器
    /// </summary>
    public class SandTableController : MonoBehaviour
    {
        /// <summary>
        /// 沙盘字典
        /// </summary>
        private Dictionary<string, SandTable> m_SandTables = null;

        private string currentName = null;
        /// <summary>
        /// 当前展示的沙盘
        /// </summary>
        public string CurrentName
        {
            get { return currentName; }
            private set { currentName = value; }
        }

        private int currentIndex =0;

        /// <summary>
        /// 当前流程 索引
        /// </summary>
        public int CurrentIndex
        {
            get { return currentIndex; }
            set { currentIndex = value; }
        }


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
            m_SandTables = new Dictionary<string, SandTable>();

            for (int i = 0; i < transform.childCount; i++)
            {
                string name = (i + 1).ToString();
                SandTable table = transform.Find(name).GetComponent<SandTable>();
                table.StagePointClicked.AddListener(x => { StagePointClicked.Invoke(x); });
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
        public void DisplayProcess(int index)
        {
            ///获取指定沙盘
            SandTable st = m_SandTables[currentName];
            ///打开指定流程
            st.DisplayProcess(index);
            CurrentIndex = index;
        }

        /// <summary>
        /// 设置关卡
        /// </summary>
        /// <param name="level"></param>
        public void SetLevelInfo(int level)
        {
            int index = 0;
            foreach (var key in m_SandTables.Keys)
            {
                foreach (var flowList in m_SandTables[key].ProcessFlowList)
                {
                    foreach (var point in flowList.points)
                    {
                        if (point.name.Contains("工艺") && index < level)
                        {
                            point.SetLockState(false);
                            index++;
                        }
                    }
                }
            }
        }
    }
}
