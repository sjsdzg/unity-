using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 工艺元件控制器
    /// </summary>
    public class ProcessElementController : MonoBehaviour
    {
        /// <summary>
        /// 工艺元件字典
        /// </summary>
        private Dictionary<string, ProcessElementComponent> m_Components = null;

        /// <summary>
        /// 工艺元件信息列表
        /// </summary>
        public List<ProcessElement> CurrentElements { get; private set; }

        private float m_Transparent = 0.2f;
        /// <summary>
        /// 透明度
        /// </summary>
        public float Transparent
        {
            get { return m_Transparent; }
            set
            {
                m_Transparent = value;
                SetBlurEffect(CurrentElements);
            }
        }

        void Awake()
        {
            //初始化工艺元件
            InitProcessElement();
        }

        /// <summary>
        /// 初始化工艺元件
        /// </summary>
        private void InitProcessElement()
        {
            m_Components = new Dictionary<string, ProcessElementComponent>();

            foreach (Transform item1 in transform)
            {
                ProcessElementComponent element1 = item1.GetComponent<ProcessElementComponent>();
                if (element1 != null)
                {
                    m_Components.Add(element1.name, element1);
                }

                //遍历第二层
                foreach (Transform item2 in item1)
                {
                    ProcessElementComponent element2 = item2.GetComponent<ProcessElementComponent>();
                    if (element2 != null)
                    {
                        m_Components.Add(element2.name, element2);
                    }
                }
            }
        }

        /// <summary>
        /// 设置虚化效果
        /// </summary>
        /// <param name="elements"></param>
        public void SetBlurEffect(List<ProcessElement> elements)
        {
            if (elements == null)
                return;

            //if (CurrentElements == elements)
            //    return;

            StopAllCoroutines();
            StartCoroutine(Blurring(elements));
        }

        IEnumerator Blurring(List<ProcessElement> elements)
        {
            yield return new WaitForEndOfFrame();

            foreach (var component in m_Components.Values)
            {
                bool b = false;
                foreach (var element in elements)
                {
                    if (element.Desc == component.name)
                    {
                        b = true;
                        break;
                    }
                }

                //虚化效果
                if (b)
                    component.Transparent = 1f;
                else
                    component.Transparent = m_Transparent;

                yield return new WaitForEndOfFrame();
            }

            CurrentElements = elements;
        }

        /// <summary>
        /// 还原
        /// </summary>
        public void Restore()
        {
            foreach (var component in m_Components.Values)
            {
                component.Transparent = 1f;
            }

            CurrentElements = null;
        }
    }
}
