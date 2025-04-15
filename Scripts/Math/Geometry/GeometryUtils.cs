using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    public class GeometryUtils
    {
        /// <summary>
        /// state
        /// -1 : 不相交
        ///  0 : 相切
        ///  1 : 相交
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="circle"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int SegmentIntersectCircle(Segment2 segment, Circle2 circle, out List<Vector2> result)
        {
            return SegmentIntersectCircle(segment.p1, segment.p2, circle.center, circle.radius, out result);
        }

        /// <summary>
        /// 线段和圆相交状态
        /// -1 : 不相交
        ///  0 : 相切
        ///  1 : 相交
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int SegmentIntersectCircle(Vector2 p1, Vector2 p2, Vector2 center, float radius, out List<Vector2> result)
        {
            int state = -1;
            result = new List<Vector2>();

            float A = (p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y);
            float B = 2 * ((p2.x - p1.x) * (p1.x - center.x) + (p2.y - p1.y) * (p1.y - center.y));
            float C = center.x * center.x + center.y * center.y + p1.x * p1.x + p1.y * p1.y - 2 * (center.x * p1.x + center.y * p1.y) - radius * radius;

            /*
             * 根据B2-4AC的结果，可以判断线段所在直线和圆的相交情况
             *    如果小于0，表示没有交点
             *    如果等于0，表示相切，只有一个交点
             *    如果大于0，表示有两个交点
             *   
             * 针对P1和P2之间的线段，根据计算出的u值，有5种结果
             *    如果线段和圆没有交点，而且都在圆的外面的话，则u的两个解都是小于0或者大于1的
             *    如果线段和圆没有交点，而且都在圆的里面的话，u的两个解符号相反，一个小于0，一个大于1
             *    如果线段和圆只有一个交点，则u值中有一个是在0和1之间，另一个不是
             *    如果线段和圆有两个交点，则u值得两个解都在0和1之间
             *   如果线段和圆相切，则u值只有1个解，且在0和1之间
             */
            float det = B * B - 4 * A * C;

            if (MathUtility.Appr(det, 0)) // 相切
            {
                state = 0;
                float t = (-B) / (2 * A);
                result.Add(p1 + t * (p2 - p1));
            }
            else if (det < 0) // 不相交
            {
                state = -1;
            }
            else // 相交
            {
                state = 1;

                float t1 = (-B - Mathf.Sqrt(det)) / (2 * A);
                float t2 = (-B + Mathf.Sqrt(det)) / (2 * A);

                if (t1 >= 0 && t1 <= 1)
                    result.Add(p1 + t1 * (p2 - p1));

                if (t2 >= 0 && t2 <= 1)
                    result.Add(p1 + t2 * (p2 - p1));
            }

            return state;
        }

        /// <summary>
        /// 线段和圆弧相交状态
        /// -1 : 不相交
        ///  0 : 相切
        ///  1 : 相交
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="arc"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int SegmentIntersectArc(Segment2 segment, Arc2 arc, out List<Vector2> result)
        {
            int state = -1;
            result = new List<Vector2>();

            float distance = 0;
            int circle_state = SegmentIntersectCircle(segment.p1, segment.p2, arc.center, arc.radius, out List<Vector2> points);
            // 和圆弧所在圆相交状态
            if (circle_state == -1) // 和圆不相交
            {
                state = -1;
            }
            else if (circle_state == 0) // 和圆相切
            {
                distance = arc.GetDistanceToPoint(points[0]);
                if (MathUtility.Appr(distance, 0))
                {
                    state = 0;
                    result.Add(points[0]);
                }
            }
            else // 和圆相交
            {
                foreach (var point in points)
                {
                    distance = arc.GetDistanceToPoint(point);
                    if (MathUtility.Appr(distance, 0))
                    {
                        state = 1;
                        result.Add(point);
                    }
                }
            }

            return state;
        }

        /// <summary>
        /// 多边形是否包含点
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool PolygonContainsPoint(List<Vector2> polygon, Vector2 point)
        {
            if (Polygon2.Side(point, polygon) <= 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool ComplexPolygonContainsPoint(List<List<Vector2>> polygon, Vector2 point)
        {
            if (ComplexPolygon2.Side(point, polygon) <= 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 扇形是否包含点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="startAngle"></param>
        /// <param name="sweepAngle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool CircularSectorContainsPoint(Vector2 center, float radius, float startAngle, float sweepAngle, Vector2 point)
        {
            Vector2 pc = point - center;

            if (pc.sqrMagnitude > radius * radius)
                return false;

            Vector2 start = new Vector2(radius * Mathf.Cos(startAngle), radius * Mathf.Sin(startAngle));
            float rad = Vector2.SignedAngle(start, pc) * Mathf.Deg2Rad;

            if (rad < 0)
                rad += 2 * Mathf.PI;

            if (rad <= sweepAngle)
                return true;

            return false;
        }

        /// <summary>
        /// 扇形是否包含点
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool CircularSectorContainsPoint(Arc2 arc, Vector2 point)
        {
            return CircularSectorContainsPoint(arc.center, arc.radius, arc.startAngle, arc.sweepAngle, point);
        }

        /// <summary>
        /// 三角形是否包含点
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool TriangleContainsPoint(Vector2 a, Vector2 b, Vector2 c, Vector2 point)
        {
            Vector2 pa = a - point;
            Vector2 pb = b - point;
            Vector2 pc = b - point;
            // 三个向量叉乘
            float t1 = pa.x * pb.y - pa.y * pb.x;
            float t2 = pb.x * pc.y - pb.y * pc.x;
            float t3 = pc.x * pa.y - pc.y * pa.x;
            // 判断是否同号
            return t1 * t2 >= 0 && t1 * t3 >= 0;
        }

        /// <summary>
        /// 可旋转矩形是否包含点
        /// 注：angle 是（degree）度
        /// </summary>
        /// <param name="center"></param>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="angle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool RectangleContainsPoint(Vector2 center, float length, float width, Vector2 direction, Vector2 point)
        {
            Vector2 normal = new Vector2(-direction.y, direction.x);

            Vector2 pc = point - center;
            float x = Vector2.Dot(pc, direction);
            float y = Vector2.Dot(pc, normal);

            if (System.Math.Abs(x) <= length * 0.5f && System.Math.Abs(y) <= width * 0.5f)
            {
                return true;
            }

            return false;
        }

        public static List<Segment2> ToSegmets(List<Vector2> points)
        {
            List<Segment2> segments = new List<Segment2>();
            int length = points.Count;
            for (int p = length -1, q = 0; q < length; p = q++)
            {
                segments.Add(new Segment2(points[p], points[q]));
            }
            return segments;
        }
    }
}
