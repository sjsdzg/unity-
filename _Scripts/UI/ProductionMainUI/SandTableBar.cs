using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 沙盘栏目
    /// </summary>
    public class SandTableBar : MonoBehaviour
    {
        public class DisplaySandTableEvent : UnityEvent<string> { }

        /// <summary>
        /// 沙盘Toggle数组
        /// </summary>
        private Toggle[] m_Toggles;

        private DisplaySandTableEvent m_DisplaySandTable = new DisplaySandTableEvent();
        /// <summary>
        /// 显示沙盘
        /// </summary>
        public DisplaySandTableEvent DisplaySandTable
        {
            get { return m_DisplaySandTable; }
            set { m_DisplaySandTable = value; }
        }


        void Awake()
        {
            m_Toggles = transform.GetComponentsInChildren<Toggle>();

            for (int i = 0; i < m_Toggles.Length; i++)
            {
                Toggle item = m_Toggles[i];
                item.onValueChanged.AddListener(x => { item_onValueChanged(item.name, x); });
            }
        }

        /// <summary>
        /// 沙盘Toggle更改时，触发
        /// </summary>
        /// <param name="name"></param>
        /// <param name="b"></param>
        private void item_onValueChanged(string name, bool b)
        {
            if (b)
            {
                DisplaySandTable.Invoke(name);
            }
        }
    }
}
