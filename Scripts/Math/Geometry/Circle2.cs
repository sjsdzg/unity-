using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    /// <summary>
    /// 2D Circle
    /// </summary>
    public struct Circle2 : IEquatable<Circle2>
    {
        /// <summary>
        /// 中心点
        /// </summary>
        public Vector2 center;

        /// <summary>
        /// 半径
        /// </summary>
        public float radius;

        /// <summary>
        /// 单位圆
        /// </summary>
        public static Circle2 Unit { get { return new Circle2(1); } }

        /// <summary>
        /// 周长
        /// </summary>
        public float Length 
        { 
            get 
            {
                return 2 * Mathf.PI * radius;
            }
        }

        /// <summary>
        /// 面积
        /// </summary>
        public float Area
        {
            get
            {
                return Mathf.PI * radius * radius;
            }
        }

        public Circle2(float radius) : this(Vector2.zero, radius)
        {

        }

        public Circle2(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        /// <summary>
        /// 根据度数取值 [0, 360]
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public Vector2 SampleDeg(float degree)
        {
            float theta = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(theta), y = Mathf.Sin(theta);
            return new Vector2(x, y);
        }

        /// <summary>
        /// 根据弧度取值 [0, 2 * PI]
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public Vector2 SampleRad(float radian)
        {
            float x = Mathf.Cos(radian), y = Mathf.Sin(radian);
            return new Vector2(x, y);
        }

        /// <summary>
        /// 取值 [0, 1]
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector2 SampleT(float t)
        {
            float theta = t * 2 * Mathf.PI;
            float x = Mathf.Cos(theta), y = Mathf.Sin(theta);
            return new Vector2(x, y);
        }

        public Vector2 TangentT(float t)
        {
            float theta = t * 2 * Mathf.PI;
            float x = Mathf.Cos(theta), y = Mathf.Sin(theta);
            Vector2 tangent = new Vector2(-y, x);
            tangent.Normalize();
            return tangent;
        }

        public bool Contains(Vector2 point)
        {
            float d = Vector2.Distance(center, point);
            return d <= radius;
        }

        public bool Equals(Circle2 other)
        {
            return this.center.Equals(other.center) && this.radius.Equals(other.radius);
        }

        public override string ToString()
        {
            return string.Format("Circle2(center: {0}, radius: {1})", center, radius);
        }
    }
}
