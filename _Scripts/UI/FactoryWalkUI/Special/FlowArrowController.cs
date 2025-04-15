using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// 流动的箭头
    /// </summary>
    public class FlowArrowController : MonoBehaviour
    {
        public FlowArrow[] arrows;

        public float Interval = 0.4f;

        private bool doing = false;

        void Start()
        {
            Transform parent = transform.Find("箭头");
            arrows = new FlowArrow[parent.childCount];

            string name = string.Empty;
            for (int i = 0; i < parent.childCount; i++)
            {
                name = string.Format("箭头 ({0})", i);
                arrows[i] = parent.Find(name).GetComponent<FlowArrow>();
            }
        }

        /// <summary>
        /// 出现
        /// </summary>
        public void Appear()
        {
            if (doing)
                return;

            StartCoroutine(Appearing());
            doing = true;
        }

        /// <summary>
        /// 消失
        /// </summary>
        public void Disappear()
        {
            StopAllCoroutines();
            doing = false;

            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i].Disappear();
            }
        }

        /// <summary>
        /// 出现协程
        /// </summary>
        /// <returns></returns>
        IEnumerator Appearing()
        {
            yield return new WaitForEndOfFrame();

            int index = 0;
            while (index <= arrows.Length)
            {
                for (int i = 0; i < arrows.Length; i++)
                {
                    if (i <= index)
                    {
                        arrows[i].Appear();
                    }
                    else
                    {
                        arrows[i].Disappear();
                    }
                }

                yield return new WaitForSeconds(Interval);
                index++;
            }

            doing = false;
        }

    }
}
