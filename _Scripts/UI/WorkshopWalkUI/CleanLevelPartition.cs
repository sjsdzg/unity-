using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// 结晶等级分区
    /// </summary>
    public class CleanLevelPartition : MonoBehaviour
    {
        private Dictionary<string, Transform> parts = new Dictionary<string, Transform>();

        void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform item = transform.GetChild(i);
                parts.Add(item.name, item);
                item.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 展示洁净区
        /// </summary>
        /// <param name="name"></param>
        public void Display(string name, bool b = true)
        {
            Transform part = null;
            if (parts.TryGetValue(name, out part))
            {
                part.gameObject.SetActive(b);
            }
        }
    }
}
