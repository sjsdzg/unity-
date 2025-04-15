using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    public struct Line3 : IEquatable<Line3>
    {
        /// <summary>
        /// 原始点
        /// </summary>
        public Vector3 origin;

        /// <summary>
        /// 方位
        /// </summary>
        public Vector3 direction;

        public Line3(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction.normalized;
        }

        public Vector3 GetPoint(float distance)
        {
            return origin + direction * distance;
        }

        /// <summary>
        /// 返回点到直线的距离
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetDistanceToPoint(Vector3 point)
        {
            float b = Vector3.Dot(point - origin, direction);
            float c = Vector3.Distance(point, origin);
            return Mathf.Sqrt(c * c - b * b);
        }

        /// <summary>
        /// 根据给定的点，返回直线上最近的点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3 ClosestPointOnLine(Vector3 point)
        {
            float distance = Vector3.Dot(point - origin, direction);
            return GetPoint(distance);
        }

        public bool Equals(Line3 other)
        {
            return this.origin.Equals(other.origin) && this.direction.Equals(other.direction);
        }

        public override string ToString()
        {
            return string.Format("Line3(origin: {0}, direction: {1})", origin, direction);
        }
    }
}
