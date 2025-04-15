using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Component;
using XFramework.Simulation;

namespace XFramework.UI
{
    /// <summary>
    /// 工艺演示组件
    /// </summary>
    public class StageFlowComponent : MonoBehaviour
    {
        /// <summary>
        /// 工艺点
        /// </summary>
        public StagePointComponent[] points;

        /// <summary>
        /// 时间间隔
        /// </summary>
        public float Interval = 0.4f;

        /// <summary>
        /// 是否在执行
        /// </summary>
        private bool doing = false;

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
            m_FocusComponent = transform.Find("Focus").GetComponent<FocusComponent>();

            Transform parent = transform.Find("流程");
            points = new StagePointComponent[parent.childCount];

            string name1 = string.Empty;
            string name2 = string.Empty;
            for (int i = 0; i < parent.childCount; i++)
            {
                name1 = string.Format("工艺_{0}", i);
                name2 = string.Format("箭头_{0}", i);

                StagePointComponent point;
                if (parent.Find(name1) != null)
                {
                    point = parent.Find(name1).GetComponent<StagePointComponent>();
                    points[i] = point;
                    //添加工艺按钮点击事件
                    point.OnClicked.AddListener(x => {
                        StagePointClicked.Invoke(x); });
                }
                else if (parent.Find(name2) != null)
                {
                    point = parent.Find(name2).GetComponent<StagePointComponent>();
                    points[i] = point;
                }
            }
        }

        /// <summary>
        /// 出现
        /// </summary>
        public void Appear()
        {
            //if (doing)
            //    return;

          //  StopAllCoroutines();
            m_FocusComponent.Focus();
           // StartCoroutine(Appearing());
            doing = true;
        }

        /// <summary>
        /// 消失
        /// </summary>
        public void Disappear()
        {
            StopAllCoroutines();
            doing = false;

            for (int i = 0; i < points.Length; i++)
            {
                points[i].Disappear();
            }
        }

        /// <summary>
        /// 出现协程
        /// </summary>
        /// <returns></returns>
        IEnumerator Appearing()
        {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < points.Length; i++)
            {
                points[i].Disappear();
            }
            yield return new WaitForSeconds(0.5f);
            int index = 0;
            while (index < points.Length)
            {
                points[index].Appear();
                //for (int i = 0; i < points.Length; i++)
                //{
                //    if (i == index)
                //    {
                //        points[i].Appear();
                //    }
                //    else if (i > index)
                //    {
                //        points[i].Disappear();
                //    }
                //}

                yield return new WaitForSeconds(Interval);
                index++;
            }

            doing = false;
        }
    }
}
