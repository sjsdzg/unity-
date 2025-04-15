using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    public class KnowledgePointController : MonoBehaviour
    {
        /// <summary>
        /// 知识点字典
        /// </summary>
        public Dictionary<string, BaseKnowledgePoint> KnowledgePoints = new Dictionary<string, BaseKnowledgePoint>();

        /// <summary>
        /// 显示的知识点
        /// </summary>
        public List<BaseKnowledgePoint> displayKnowledgePoints = new List<BaseKnowledgePoint>();

        void Start()
        {
            BaseKnowledgePoint[] points = transform.GetComponentsInChildren<BaseKnowledgePoint>();

            for (int i = 0; i < points.Length; i++)
            {
                BaseKnowledgePoint point = points[i];
                point.Close();
                KnowledgePoints.Add(point.name, point);
            }
        }

        /// <summary>
        /// 展示知识点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isDisableOthers"></param>
        public void DisplayKnowledgePoint(string name, bool isCloseOthers = true)
        {
            BaseKnowledgePoint point = null;

            if (isCloseOthers)
            {
                foreach (var item in displayKnowledgePoints)
                {
                    item.Close();
                }

                displayKnowledgePoints.Clear();
            }

            if (KnowledgePoints.TryGetValue(name, out point))
            {
                point.Display();
                displayKnowledgePoints.Add(point);
            }
        }

        /// <summary>
        /// 关闭知识点
        /// </summary>
        /// <param name="name"></param>
        public void CloseKnowledgePoint(string name)
        {
            BaseKnowledgePoint point = null;

            if (KnowledgePoints.TryGetValue(name, out point))
            {
                point.Close();
                displayKnowledgePoints.Remove(point);
            }
        }

        /// <summary>
        /// 关闭所有知识点
        /// </summary>
        public void CloseAllKnowledgePoint()
        {
            foreach (var item in displayKnowledgePoints)
            {
                item.Close();
            }
        }
    }
}
