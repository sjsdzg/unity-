using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    /// <summary>
    /// 2D Line
    /// </summary>
    public struct Line2 : IEquatable<Line2>
    {
        public Vector2 p1;

        public Vector2 p2;

        public Vector2 direction
        {
            get
            {
                return (p2 - p1).normalized;
            }
        }

        /// <summary>
        /// 直线的逆时针方向的法线
        /// </summary>
        public Vector2 normal
        {
            get
            {
                //Ray
                Vector2 vector = (p2 - p1).normalized;
                return new Vector2(-vector.y, vector.x);
            }
        }

        public Line2(float x1, float y1, float x2, float y2)
        {
            p1 = new Vector2(x1, y1);
            p2 = new Vector2(x2, y2);
        }

        public Line2(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        /// <summary>
        /// 平移直线
        /// 1.按逆时针方向的法线，平移distance的直线。
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Line2 Move(float distance)
        {
            Vector2 vector = normal * distance;
            return new Line2(p1 + vector, p2 + vector);
        }

        /// <summary>
        /// 判断两条直线是否相交，并求出交点。
        /// </summary>
        /// <param name="other"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Intersect(Line2 other, out Vector2 point)
        {
            return Intersect(this, other, out point);
        }

        /// <summary>
        /// 判断两条直线是否相交，并求出交点。
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool Intersect(Line2 line1, Line2 line2, out Vector2 point)
        {
            return Intersect(line1.p1, line1.p2, line2.p1, line2.p2, out point);
        }

        /// <summary>
        /// 判断两条直线是否相交，并求出交点。
        /// </summary>
        /// <param name="point1_1">第1条直线的第1个点</param>
        /// <param name="point1_2">第1条直线的第2个点</param>
        /// <param name="point2_1">第2条直线的第1个点</param>
        /// <param name="point2_2">第2条直线的第2个点</param>
        /// <param name="point">交点</param>
        /// <returns></returns>
        public static bool Intersect(Vector2 point1_1, Vector2 point1_2, Vector2 point2_1, Vector2 point2_2, out Vector2 point)
        {
            var a1 = point1_2.y - point1_1.y;
            var b1 = point1_1.x - point1_2.x;

            var a2 = point2_2.y - point2_1.y;
            var b2 = point2_1.x - point2_2.x;

            var det = a1 * b2 - a2 * b1;
            if (MathUtility.Appr(det, 0))
            {
                point = new Vector2(float.NaN, float.NaN);
                return false; // Parallel, no intersection
            }

            var c1 = a1 * point1_1.x + b1 * point1_1.y;
            var c2 = a2 * point2_1.x + b2 * point2_1.y;
            var detInv = 1.0f / det;
            point = new Vector2((b2 * c1 - b1 * c2) * detInv, (a1 * c2 - a2 * c1) * detInv);
            return true;
        }

        /// <summary>
        /// 点到直线的距离
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetDistanceToPoint(Vector2 point)
        {
            return GetDistanceToPoint(this, point);
        }

        /// <summary>
        /// 点到直线的距离
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float GetDistanceToPoint(Line2 line, Vector2 point)
        {
            float area = MathUtility.Cross(point - line.p1, line.p2 - line.p1);
            return Mathf.Abs(area) / (line.p2 - line.p1).magnitude;
        }

        /// <summary>
        /// 根据给定的点，返回直线上最近的点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 ClosestPointOnLine(Vector2 point)
        {
            return ClosestPointOnLine(point, p1, p2);
        }

        /// <summary>
        /// 根据给定的点，返回直线上最近的点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 ClosestPointOnLine(Vector2 point, Vector2 a, Vector2 b)
        {
            float x = a.x;
            float y = a.y;
            float dx = b.x - x;
            float dy = b.y - y;

            if (dx != 0 || dy != 0)
            {
                float t = ((point.x - x) * dx + (point.y - y) * dy) / (dx * dx + dy * dy);
                x += dx * t;
                y += dy * t;
            }

            return new Vector2(x, y);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", p1, p2);
        }

        public string ToString(string format)
        {
            return string.Format("({0}, {1})", p1.ToString(format), p2.ToString(format));
        }

        public bool Equals(Line2 other)
        {
            var a1 = this.p2.y - this.p1.y;
            var b1 = this.p1.x - this.p2.x;

            var a2 = other.p2.y - other.p2.y;
            var b2 = other.p1.x - other.p2.x;

            var c1 = a1 * this.p1.x + b1 * this.p1.y;
            var c2 = a2 * other.p1.x + b2 * other.p1.y;

            var cross1 = a1 * b2 - a2 * b1;
            var cross2 = a1 * c2 - a2 * c1;

            if (Mathf.Abs(cross1) <= Mathf.Epsilon && Mathf.Abs(cross2) <= Mathf.Epsilon)
                return true;

            return false;
        }

        public static implicit operator Segment2(Line2 line)
        {
            return new Segment2(line.p1, line.p2);
        }
    }
}
