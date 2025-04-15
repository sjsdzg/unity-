using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Core;
using System;

namespace XFramework.Common
{
    [RequireComponent(typeof(LineRenderer))]
    public class DynamicLine : MonoBehaviour
    {
        /// <summary>
        /// LineRenderer
        /// </summary>
        private LineRenderer m_LineRenderer;

        /// <summary>
        /// Points
        /// </summary>
        private List<Vector3> m_LinePoints = new List<Vector3>();

        /// <summary>
        /// 子物体列表
        /// </summary>
        public List<Transform> items = new List<Transform>();

        /// <summary>
        /// 过渡时间
        /// </summary>
        public float m_Duration = 1f;

        /// <summary>
        /// 自动播放
        /// </summary>
        public bool m_OnStartDraw = true;

        /// <summary>
        /// 动态绘制
        /// </summary>
        public bool m_DynamicDrawing = true;

        private void Awake()
        {
            m_LineRenderer = transform.GetComponent<LineRenderer>();
            items.ForEach(item => m_LinePoints.Add(item.position));
        }

        private void Start()
        {
            if (m_OnStartDraw)
            {
                if (m_DynamicDrawing)
                {
                    DynamicDraw(m_LinePoints, m_Duration);
                }
                else
                {
                    Draw(m_LinePoints);
                }
            }
        }

        /// <summary>
        /// 绘制线条
        /// </summary>
        public void Draw()
        {
            if (m_DynamicDrawing)
            {
                DynamicDraw(m_LinePoints, m_Duration);
            }
            else
            {
                Draw(m_LinePoints);
            }
        }

        /// <summary>
        /// 绘制线条
        /// </summary>
        /// <param name="points"></param>
        private void Draw(List<Vector3> points)
        {
            m_LineRenderer.positionCount = m_LinePoints.Count;
            m_LineRenderer.SetPositions(m_LinePoints.ToArray());
        }

        /// <summary>
        /// 在一定的时间范围内，动态绘制线条
        /// </summary>
        /// <param name="points"></param>
        /// <param name="duration"></param>
        private void DynamicDraw(List<Vector3> points, float duration)
        {
            StopAllCoroutines();
            StartCoroutine(Drawing(m_LinePoints, duration));
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            StopAllCoroutines();
            m_LineRenderer.positionCount = 0;
        }

        /// <summary>
        /// 在一定的时间范围内，动态绘制
        /// </summary>
        /// <param name="points"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        IEnumerator Drawing(List<Vector3> points, float duration)
        {
            float distance = GetDistance(points);

            if (distance > 0)
            {
                m_LineRenderer.positionCount = 1;
                m_LineRenderer.SetPosition(0, points[0]);
                for (int i = 0; i < points.Count - 1; i++)
                {
                    m_LineRenderer.positionCount = i + 2;
                    float _distance = Vector3.Distance(points[i], points[i + 1]);
                    float _duration = _distance / distance * duration;

                    float lerp = 0f;
                    Vector3 _point = Vector3.zero;
                    while (lerp < 1)
                    {
                        lerp += Time.fixedDeltaTime / _duration;
                        _point = Vector3.LerpUnclamped(points[i], points[i + 1], lerp);
                        m_LineRenderer.SetPosition(i + 1, _point);
                        yield return new WaitForFixedUpdate();
                    }
                }
            }
        }

        /// <summary>
        /// 获取距离
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public float GetDistance(List<Vector3> points)
        {
            List<float> ratios = new List<float>();
            float distance = 0;

            if (points == null)
            {
                distance = 0;
            }
            else if (points.Count < 2)
            {
                distance = 0;
            }
            else
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    distance += Vector3.Distance(points[i], points[i + 1]);
                }
            }

            return distance;
        }

        /// <summary>
        /// 设置子物体
        /// </summary>
        [ContextMenu("SetChildren")]
        private void SetChildren()
        {
            items.Clear();
            int n = 0;
            foreach (Transform child in transform)
            {
                child.name = "Linepoint " + (n++).ToString("000");
                items.Add(child);
            }
        }
    }
}

