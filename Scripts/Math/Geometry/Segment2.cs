using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    /// <summary>
    /// 2D Line Segments
    /// </summary>
    public struct Segment2 : IEquatable<Segment2>
    {
        public Vector2 p1;

        public Vector2 p2;

        /// <summary>
        /// 线段的长度
        /// </summary>
        public float magnitude
        {
            get { return (p2 - p1).magnitude; }
        }

        /// <summary>
        /// 线段的逆时针方向的单位向量
        /// </summary>
        public Vector2 direction
        {
            get
            {
                return (p2 - p1).normalized;
            }
        }

        /// <summary>
        /// 线段的逆时针方向的法线
        /// </summary>
        public Vector2 normal
        {
            get
            {
                Vector2 vector = (p2 - p1).normalized;
                return new Vector2(-vector.y, vector.x);
            }
        }

        public Segment2(float x1, float y1, float x2, float y2)
        {
            p1 = new Vector2(x1, y1);
            p2 = new Vector2(x2, y2);
        }

        public Segment2(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        /// <summary>
        /// 平移线段
        /// 1.按逆时针方向的法线，平移distance的线段。
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Segment2 Move(float distance)
        {
            Vector2 vector = normal * distance;
            return new Segment2(p1 + vector, p2 + vector);
        }

        /// <summary>
        /// 切除两端
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public Segment2 Trim(float delta)
        {
            Vector2 direction = (p2 - p1).normalized;
            return new Segment2(p1 + direction * delta, p2 - direction * delta);
        }

        /// <summary>
        /// 延伸线段
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public Segment2 Extend(float delta)
        {
            Vector2 direction = (p2 - p1).normalized;
            return new Segment2(p1 - direction * delta, p2 + direction * delta);
        }

        /// <summary>
        /// 延伸线段
        /// </summary>
        /// <param name="delta1"></param>
        /// <param name="delta2"></param>
        /// <returns></returns>
        public Segment2 Extend(float delta1, float delta2)
        {
            Vector2 direction = (p2 - p1).normalized;
            return new Segment2(p1 - direction * delta1, p2 + direction * delta2);
        }

        /// <summary>
        /// 判断两条线段两条线段的是否重合
        /// 返回重合部分
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlap(Segment2 other, out Segment2 result)
        {
            Vector2 dir = this.direction;
            float dot = Vector2.Dot(other.direction, dir);
            if (dot < 0)
            {
                Vector2 temp = other.p1;
                other.p1 = other.p2;
                other.p2 = temp;
            }
            // 距离
            float distance = Line2.GetDistanceToPoint(other, p1);
            if (!MathUtility.Appr(distance, 0))
            {
                result = new Segment2();
                return false;
            }
            // 是否平行
            if (!MathUtility.Appr(dir, other.direction))
            {
                result = new Segment2();
                return false;
            }

            int c11 = Compare(p1, other.p1, dir);
            int c12 = Compare(p1, other.p2, dir);
            int c21 = Compare(p2, other.p1, dir);
            int c22 = Compare(p2, other.p2, dir);

            if (c11 <= 0 && c22 >= 0)
            {
                result = new Segment2(p1, p2);
                return true;
            }
            else if (c11 >= 0 && c22 <= 0)
            {
                result = new Segment2(other.p1, other.p2);
                return true;
            }
            else if (c11 <= 0 && c12 >= 0)
            {
                result = new Segment2(p1, other.p2);
                return true;
            }
            else if (c21 <= 0 && c22 >= 0)
            {
                result = new Segment2(other.p1, p2);
                return true;
            }
            else
            {
                result = new Segment2();
                return false;
            }
        }

        public bool Union(Segment2 other, out Segment2 result)
        {
            Vector2 dir = this.direction;
            float dot = Vector2.Dot(other.direction, dir);
            if (dot < 0)
            {
                Vector2 temp = other.p1;
                other.p1 = other.p2;
                other.p2 = temp;
            }
            // result
            result = new Segment2();
            // 距离
            float distance = Line2.GetDistanceToPoint(other, p1);
            if (!MathUtility.Appr(distance, 0))
            {
                return false;
            }
            // 是否平行
            if (!MathUtility.Appr(dir, other.direction))
            {
                return false;
            }

            int c11 = Compare(p1, other.p1, dir);
            if (c11 > 0)
                result.p1 = p1;
            else
                result.p1 = other.p1;

            int c22 = Compare(p2, other.p2, dir);
            if (c22 > 0)
                result.p2 = other.p2;
            else
                result.p2 = p2;

            return true;
        }

        public int Compare(Vector2 point1, Vector2 point2, Vector2 direction)
        {
            if (point1.Equals(point2))
                return 0;

            Vector2 dir = (point2 - point1).normalized;

            float dot = Vector2.Dot(dir, direction);
            if (dot > 0)
                return 1;
            else
                return -1;
        }

        /// <summary>
        /// 线段偏移，增量delta
        /// 4个点，按顺时针排列
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public Vector2[] Offset(float delta)
        {
            Vector2[] points = new Vector2[4];

            Vector2 vector = normal * delta;
            points[0] = p1 + vector;
            points[1] = p2 + vector;
            points[2] = p2 - vector;
            points[3] = p1 - vector;

            return points;
        }

        /// <summary>
        /// 判断两条线段是否相交，并求出交点。
        /// </summary>
        /// <param name="other"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Intersect(Segment2 other, out Vector2 point)
        {
            return Intersect(this, other, out point);
        }

        /// <summary>
        /// 判断两条线段是否相交，并求出交点。
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool Intersect(Segment2 line1, Segment2 line2, out Vector2 point)
        {
            return Intersect(line1.p1, line1.p2, line2.p1, line2.p2, out point);
        }

        /// <summary>
        /// 判断两条线段是否相交，并求出交点。
        /// </summary>
        /// <param name="point1_1"></param>
        /// <param name="point1_2"></param>
        /// <param name="point2_1"></param>
        /// <param name="point2_2"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool Intersect(Vector2 point1_1, Vector2 point1_2, Vector2 point2_1, Vector2 point2_2, out Vector2 point)
        {
            // 是否交叉
            bool cross = true; 
            // 快速排斥实验
            if (Mathf.Min(point1_1.x, point1_2.x) > Mathf.Max(point2_1.x, point2_2.x)) cross = false;
            if (Mathf.Min(point2_1.x, point2_2.x) > Mathf.Max(point1_1.x, point1_2.x)) cross = false;
            if (Mathf.Min(point1_1.y, point1_2.y) > Mathf.Max(point2_1.y, point2_2.y)) cross = false;
            if (Mathf.Min(point2_1.y, point2_2.y) > Mathf.Max(point1_1.y, point1_2.y)) cross = false;
            // 跨立实验
            if (MathUtility.Cross(point1_1 - point2_1, point2_2 - point2_1) * MathUtility.Cross(point2_2 - point2_1, point1_2 - point2_1) < 0) cross = false;
            if (MathUtility.Cross(point2_1 - point1_1, point1_2 - point1_1) * MathUtility.Cross(point1_2 - point1_1, point2_2 - point1_1) < 0) cross = false;

            if (!cross)
            {
                // 线段没有相交
                point = new Vector2(float.NaN, float.NaN);
                return false;
            }

            // 解线性方程组, 求线段交点.
            // 如果分母为0 则平行或共线, 不相交  
            var denominator = (point1_2.y - point1_1.y) * (point2_2.x - point2_1.x) - (point1_1.x - point1_2.x) * (point2_1.y - point2_2.y);
            if (MathUtility.Appr(denominator, 0))
            {
                // 线段没有相交
                point = new Vector2(float.NaN, float.NaN); ;
                return false;
            }

            // 线段所在直线的交点坐标 (x , y)      
            point.x = ((point1_2.x - point1_1.x) * (point2_2.x - point2_1.x) * (point2_1.y - point1_1.y)
                       + (point1_2.y - point1_1.y) * (point2_2.x - point2_1.x) * point1_1.x
                       - (point2_2.y - point2_1.y) * (point1_2.x - point1_1.x) * point2_1.x) / denominator;
            point.y = -((point1_2.y - point1_1.y) * (point2_2.y - point2_1.y) * (point2_1.x - point1_1.x)
                        + (point1_2.x - point1_1.x) * (point2_2.y - point2_1.y) * point1_1.y
                        - (point2_2.x - point2_1.x) * (point1_2.y - point1_1.y) * point2_1.y) / denominator;

            return true;
        }

        /// <summary>
        /// 是否同向
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool SameDirection(Segment2 other)
        {
            if (p1 == other.p2 || p2 == other.p1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 点到线段的距离
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetDistanceToPoint(Vector2 point)
        {
            return GetDistanceToPoint(point, p1, p2);
        }

        /// <summary>
        /// 根据给定的点，返回线段上最近的点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 ClosestPointOnPlane(Vector2 point)
        {
            return ClosestPointOnPlane(point, p1, p2);
        }

        /// <summary>
        /// 点到线段的距离
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float GetDistanceToPoint(Vector2 point, Vector2 a, Vector2 b)
        {
            float x = a.x;
            float y = a.y;
            float dx = b.x - x;
            float dy = b.y - y;

            if (dx != 0 || dy != 0)
            {
                float t = ((point.x - x) * dx + (point.y - y) * dy) / (dx * dx + dy * dy);
                if (t > 1)
                {
                    x = b.x;
                    y = b.y;
                }
                else if (t > 0)
                {
                    x += dx * t;
                    y += dy * t;
                }
            }

            dx = point.x - x;
            dy = point.y - y;

            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// 根据给定的点，返回线段上最近的点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 ClosestPointOnPlane(Vector2 point, Vector2 a, Vector2 b)
        {
            float x = a.x;
            float y = a.y;
            float dx = b.x - x;
            float dy = b.y - y;

            if (dx != 0 || dy != 0)
            {
                float t = ((point.x - x) * dx + (point.y - y) * dy) / (dx * dx + dy * dy);
                if (t > 1)
                {
                    x = b.x;
                    y = b.y;
                }
                else if (t > 0)
                {
                    x += dx * t;
                    y += dy * t;
                }
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

        public bool Equals(Segment2 other)
        {
            return p1.Equals(other.p1) && p2.Equals(other.p2);
        }

        public static implicit operator Line2(Segment2 lineSegment2)
        {
            return new Line2(lineSegment2.p1, lineSegment2.p2);
        }

        /// <summary>
        /// 两个线段的端点是否重合
        /// </summary>
        /// <param name="other"></param>        /// <returns></returns>
        public bool OverlapPoint(Segment2 other)
        {
            return MathUtility.Appr(p1, other.p1)
                || MathUtility.Appr(p1, other.p2)
                || MathUtility.Appr(p2, other.p1)
                || MathUtility.Appr(p2, other.p2);
        }


        /// <summary>
        /// 两个线段的端点是否重合
        /// </summary>
        /// <param name="other"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool OverlapPoint(Segment2 other, out Vector2 point)
        {
            bool flag = false;
            if (MathUtility.Appr(p1, other.p1) 
                || MathUtility.Appr(p1, other.p2))
            {
                point = p1;
                flag = true;
            }
            else if (MathUtility.Appr(p2, other.p1) 
                || MathUtility.Appr(p2, other.p2))
            {
                point = p2;
                flag = true;
            }
            else
            {
                point = Vector2.negativeInfinity;
            }
            return flag;
        }

        /// <summary>
        /// 线段的端点是否和点重合
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool OverlapPoint(Vector2 point)
        {
            if (MathUtility.Appr(p1, point) 
                || MathUtility.Appr(p2, point))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取另一个
        /// </summary>
        /// <param name="corner"></param>
        /// <returns></returns>
        public Vector2 Another(Vector2 point)
        {
            if (p1 == point)
            {
                return p2;
            }
            else
            {
                return p1;
            }
        }
    }
}
