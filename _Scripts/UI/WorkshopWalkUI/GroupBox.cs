using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class GroupBox : MonoBehaviour
    {
        /// <summary>
        /// Toggle数组
        /// </summary>
        public Toggle[] m_Toggles;

        public class OnChangedEvent : UnityEvent<string, bool> { }

        private OnChangedEvent m_OnChanged = new OnChangedEvent();
        /// <summary>
        /// 发生改变事件
        /// </summary>
        public OnChangedEvent OnChanged
        {
            get { return m_OnChanged; }
            set { m_OnChanged = value; }
        }

        void Awake()
        {
            m_Toggles = transform.GetComponentsInChildren<Toggle>();

            for (int i = 0; i < m_Toggles.Length; i++)
            {
                Toggle item = m_Toggles[i];

                item.onValueChanged.AddListener(x =>
                {
                    item_onValueChanged(item.GetComponentInChildren<Text>().text, x);
                });
            }
        }

        /// <summary>
        /// Toggle改变时，触发
        /// </summary>
        /// <param name="b"></param>
        /// <param name="name"></param>
        private void item_onValueChanged(string name, bool b)
        {
            m_OnChanged.Invoke(name, b);
        }
    }
}
