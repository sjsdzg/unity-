using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Component;
using XFramework.Module;
using XFramework.Simulation;

namespace XFramework.UI
{
    /// <summary>
    /// 子工艺组件
    /// </summary>
    public class SubprocessComponent : MonoBehaviour
    {
        /// <summary>
        /// 工艺点
        /// </summary>
        public ProcessLabelComponent[] labels;

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
        private FocusComponent m_Focus;

        /// <summary>
        /// 子工艺信息
        /// </summary>
        public SubprocessInfo Data { get; private set; }

        void Awake()
        {
            m_Focus = transform.Find("Focus").GetComponent<FocusComponent>();

            Transform parent = transform.Find("标签列表");
            labels = parent.GetComponentsInChildren<ProcessLabelComponent>();
        }

        void Start()
        {
            foreach (var label in labels)
            {
                label.Disappear();
            }
        }

        /// <summary>
        /// 设置子工艺信息
        /// </summary>
        /// <param name="info"></param>
        private void SetValue(SubprocessInfo info)
        {
            Data = info;
        }

        /// <summary>
        /// 出现标签
        /// </summary>
        public void Appear()
        {
            if (doing)
                return;

            gameObject.SetActive(true);
            m_Focus.Focus();
            StartCoroutine(Appearing());
            doing = true;
        }

        /// <summary>
        /// 消失标签
        /// </summary>
        public void Disappear()
        {
            StopAllCoroutines();
            doing = false;

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Disappear();
            }

            gameObject.gameObject.SetActive(false);
        }

        /// <summary>
        /// 出现协程
        /// </summary>
        /// <returns></returns>
        IEnumerator Appearing()
        {
            yield return new WaitForSeconds(0.5f);

            int index = 0;
            while (index <= labels.Length)
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    if (i <= index)
                    {
                        yield return new WaitForEndOfFrame();
                        labels[i].Appear();
                    }
                    else
                    {
                        labels[i].Disappear();
                    }
                }

                yield return new WaitForSeconds(Interval);
                index++;
            }

            doing = false;
        }
    }
}
