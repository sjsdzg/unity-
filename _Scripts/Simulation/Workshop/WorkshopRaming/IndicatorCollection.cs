using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class IndicatorCollection : MonoBehaviour
    {
        public Dictionary<string, IndicatorComponent> m_Indicators = new Dictionary<string, IndicatorComponent>();

        private void Awake()
        {
            InitIndicators();
        }

        /// <summary>
        /// 初始化指示器
        /// </summary>
        private void InitIndicators()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                IndicatorComponent indicator = transform.GetChild(i).GetComponent<IndicatorComponent>();
                if (indicator != null)
                {
                    m_Indicators.Add(indicator.name, indicator);
                }
            }

            CloseAll();
        }

        /// <summary>
        /// 显示指示器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="closeOthers">是否关闭其他</param>
        public void Show(string name, bool closeOthers = true)
        {
            IndicatorComponent indicator = null;
            if (m_Indicators.TryGetValue(name, out indicator))
            {
                indicator.show();
                //关闭其他
                if (closeOthers)
                {
                    foreach (string item in m_Indicators.Keys)
                    {
                        if (!name.Equals(item))
                        {
                            m_Indicators[item].hide();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 关闭指示器
        /// </summary>
        /// <param name="name"></param>
        public void Close(string name)
        {
            foreach (string item in m_Indicators.Keys)
            {
                if (name.Equals(item))
                {
                    m_Indicators[item].hide();
                }
            }
        }

        /// <summary>
        /// 关闭所有指示器
        /// </summary>
        public void CloseAll()
        {
            foreach (string item in m_Indicators.Keys)
            {
                m_Indicators[item].hide();
            }
        }
    }
}

