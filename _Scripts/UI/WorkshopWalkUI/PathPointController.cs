using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 路径点控制器
    /// </summary>
    public class PathPointController : MonoBehaviour
    {
        /// <summary>
        /// 小地图要显示的标记项
        /// </summary>
        private List<MiniMapItem> miniMapItems = new List<MiniMapItem>();

        /// <summary>
        /// 显示间隔时间
        /// </summary>
        private float interval = 0.5f;

        private UnityEvent m_OnCompleted = new UnityEvent();
        /// <summary>
        /// 播放结束事件
        /// </summary>
        public UnityEvent OnCompleted
        {
            get { return m_OnCompleted; }
            set { m_OnCompleted = value; }
        }

        void Awake()
        {
            string name = string.Empty;
            for (int i = 0; i < transform.childCount; i++)
            {
                name = string.Format("pathpoint ({0})", i);
                MiniMapItem item = transform.Find(name).GetComponent<MiniMapItem>();

                if (item != null)
                {
                    miniMapItems.Add(item);
                }
            }
        }

        /// <summary>
        /// 播放路径点
        /// </summary>
        public void Play()
        {
            StartCoroutine(Playing());
        }

        IEnumerator Playing()
        {
            int i = 0;
            while (i < miniMapItems.Count)
            {
                miniMapItems[i].ShowMark();
                i++;
                yield return new WaitForSeconds(interval);
            }

            OnCompleted.Invoke();
        }

        /// <summary>
        /// 停止播放路径点
        /// </summary>
        public void Stop()
        {
            StopAllCoroutines();

            for (int i = 0; i < miniMapItems.Count; i++)
            {
                miniMapItems[i].HideMark();
            }
        }
    }
}
