using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Math
{
    public struct Polygon2 : IEquatable<Polygon2>
    {
        public List<Vector2> points;

        public Polygon2(List<Vector2> points)
        {
            this.points = points;
        }

        /// <summary>
        /// 面积
        /// 1.面积为负：多边形的点按照顺时针排列
        /// 2.面积为正：多边形的点按照逆时针排列
        /// </summary>
        public float Area
        {
            get
            {
                return CalculateArea(points);
            }
        }

        /// <summary>
        /// 方向
        /// </summary>
        public ClockDirection Direction
        {
            get
            {
                if (Area < 0)
                {
                    return ClockDirection.Clockwise;
                }
                else
                {
                    return ClockDirection.CounterClockwise;
                }
            }
        }

        /// <summary>
        /// 获取多边形的边界
        /// </summary>
        /// <returns></returns>
        public Rect GetBounds()
        {
            return GetBounds(points);
        }

        /// <summary>
        /// 获取多边形的边界
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Rect GetBounds(List<Vector2> points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (var point in points)
            {
                minX = Mathf.Min(minX, point.x);
                minY = Mathf.Min(minY, point.y);
                maxX = Mathf.Max(maxX, point.x);
                maxY = Mathf.Max(maxY, point.y);
            }
            float width = maxX - minX;
            float height = maxY - minY;
            return new Rect(minX, minY, width, height);
        }

        public Vector2 GetCentroid()
        {
            return GetCentroid(points);
        }

        /// <summary>
        /// 获取多边形的行心
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Vector2 GetCentroid(List<Vector2> points)
        {
            float area = 0.0f;// 多边形面积
            float gx = 0.0f, gy = 0.0f;// 重心的x,y
            for (int i = 1; i < points.Count; i++)
            {
                float cross = MathUtility.Cross(points[i - 1], points[i]);
                area += cross;
                gx += cross * (points[i - 1].x + points[i].x) / 3.0f;
                gy += cross * (points[i - 1].y + points[i].y) / 3.0f;
            }
            if (area == 0)
            {
                return new Vector2(points[0].x, points[0].y);
            }

            gx /= area;
            gy /= area;
            return new Vector2(gx, gy);
        }


        /// <summary>
        /// 获取视心（用于在多边形上优化文本标签的位置。）
        /// </summary>
        /// <returns></returns>
        public Vector2 GetVisualPoint(float precision)
        {
            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();

            Rect rect = Polygon2.GetBounds(points);
            float cellSize = Mathf.Min(rect.width, rect.height);
            float half = cellSize / 2;

            if (cellSize == 0)
                return rect.min;

            // cover polygon with initial cells
            List<Cell> cells = ListPool<Cell>.Get();
            for (float x = rect.min.x; x < rect.max.x; x += cellSize)
            {
                for (float y = rect.min.y; y < rect.max.y; y += cellSize)
                {
                    cells.Add(new Cell(x + half, y + half, half, this));
                }
            }

            // take centroid as the first best guess
            Vector2 centroid = Polygon2.GetCentroid(points);
            Cell bestCell = new Cell(centroid, 0, this);

            // special case for rectangular polygons
            Cell boxCell = new Cell(rect.center, 0, this);
            if (boxCell.distance > bestCell.distance)
                bestCell = boxCell;

            while (cells.Count > 0)
            {
                Cell cell = new Cell();
                float min = float.MinValue;
                foreach (var item in cells)
                {
                    if (item.maxDistance > min)
                    {
                        min = item.maxDistance;
                        cell = item;
                    }
                }
                cells.Remove(cell);

                // update the best cell if we found a better one
                if (cell.distance > bestCell.distance)
                {
                    bestCell = cell;
                }

                // do not drill down further if there's no chance of a better solution
                if (cell.maxDistance - bestCell.distance <= precision)
                    continue;

                // split the cell into four cells
                half = cell.half / 2;
                cells.Add(new Cell(cell.center.x - half, cell.center.y - half, half, this));
                cells.Add(new Cell(cell.center.x + half, cell.center.y - half, half, this));
                cells.Add(new Cell(cell.center.x - half, cell.center.y + half, half, this));
                cells.Add(new Cell(cell.center.x + half, cell.center.y + half, half, this));
            }

            ListPool<Cell>.Release(cells);
            //stopwatch.Stop();
            //Debug.LogFormat("---获取视心，耗时：{0}【毫秒】", stopwatch.ElapsedMilliseconds);

            return bestCell.center;
        }

        /// <summary>
        /// 计算多边形面积
        /// 1.面积为负：多边形的点按照顺时针排列
        /// 2.面积为正：多边形的点按照逆时针排列
        /// </summary>
        /// <returns></returns>
        public static float CalculateArea(List<Vector2> points)
        {
            int length = points.Count;
            if (length < 3) return 0.0f;

            float area = 0.0f;

            for (int p = length - 1, q = 0; q < length; p = q++)
            {
                area += MathUtility.Cross(points[p], points[q]);
            }

            return area * 0.5f;
        }

        /// <summary>
        /// signed distance from point to polygon outline
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetDistanceToPoint(Vector2 point)
        {
            return GetDistanceToPoint(point, points);
        }

        /// <summary>
        /// signed distance from point to polygon outline (negative if point is outside)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetSignDistanceToPoint(Vector2 point)
        {
            return GetSignDistanceToPoint(point, points);
        }

        /// <summary>
        /// distance from point to polygon outline
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float GetDistanceToPoint(Vector2 point, List<Vector2> polygon)
        {
            float distance = float.MaxValue;
            for (int i = 0, length = polygon.Count, j = length - 1; i < length; j = i++)
            {
                Vector2 a = polygon[i];
                Vector2 b = polygon[j];
                Segment2 segment = new Segment2(a, b);
                distance = Mathf.Min(distance, segment.GetDistanceToPoint(point));
            }
            return distance;
        }

        /// <summary>
        /// signed distance from point to polygon outline (negative if point is outside)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float GetSignDistanceToPoint(Vector2 point, List<Vector2> polygon)
        {
            bool inside = false;
            float distance = float.MaxValue;

            for (int i = 0, length = polygon.Count, j = length - 1; i < length; j = i++)
            {
                Vector2 a = polygon[i];
                Vector2 b = polygon[j];
                if (point.y < a.y != point.y < b.y)
                {
                    float x = (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x;

                    if (point.x < x)
                    {
                        inside = !inside;
                    }
                }

                distance = Mathf.Min(distance, Segment2.GetDistanceToPoint(point, a, b));
            }
            return (inside ? 1 : -1) * distance;
        }

        /// <summary>
        /// 点是否在多边形内部
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public bool Inside(Vector2 point)
        {
            return Inside(point, points);
        }

        /// <summary>
        /// 点是否在多边形外部
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public bool Outside(Vector2 point)
        {
            return Outside(point, points);
        }


        /// <summary>
        /// 点是否在多边形上
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public bool Onside(Vector2 point)
        {
            return Onside(point, points);
        }

        /// <summary>
        /// 点是否在多边形内部
        /// 射线法判断点是否在多边形内部
        /// 
        /// 以点P为端点，向左方作射线L，由于多边形是有界的。
        /// 因而当L和多边形的交点数目是奇数的时候，P在多边形内，是偶数，则P在多边形外。
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool Inside(Vector2 point, List<Vector2> polygon)
        {
            int length = polygon.Count;
            if (length < 3)
                throw new ArgumentException("polygon can not have less than 3 point");

            if (Onside(point, polygon))
            {
                return false;
            }

            bool inside = false;
            for (int i = 0, j = length - 1; i < length; j = i++)
            {
                Vector2 a = polygon[i];
                Vector2 b = polygon[j];
                if (point.y < a.y != point.y < b.y)
                {
                    float x = (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x;

                    if (point.x < x)
                    {
                        inside = !inside;
                    }
                }
            }

            return inside;
        }

        /// <summary>
        /// 点是否在多边形外部
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool Outside(Vector2 point, List<Vector2> polygon)
        {
            int length = polygon.Count;
            if (length < 3)
                throw new ArgumentException("polygon can not have less than 3 point");

            if (Onside(point, polygon))
            {
                return false;
            }

            bool outside = true;
            for (int i = 0, j = length - 1; i < length; j = i++)
            {
                Vector2 a = polygon[i];
                Vector2 b = polygon[j];
                if (point.y < a.y != point.y < b.y)
                {
                    float x = (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x;

                    if (point.x < x)
                    {
                        outside = !outside;
                    }
                }
            }

            return outside;
        }

        /// <summary>
        /// 点是否在多边形上
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool Onside(Vector2 point, List<Vector2> polygon)
        {
            float distance = GetDistanceToPoint(point, polygon);

            if (MathUtility.Appr(distance, 0))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 点和多边形的关系
        /// 1 ：点在多边形外部
        /// 0 ：点在多边形的边上
        /// -1：点在多边形内部
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static int Side(Vector2 point, List<Vector2> polygon)
        {
            int length = polygon.Count;
            if (length < 3)
                throw new ArgumentException("polygon can not have less than 3 point");

            if (Onside(point, polygon))
            {
                return 0;
            }

            bool outside = true;
            for (int i = 0, j = length - 1; i < length; j = i++)
            {
                Vector2 a = polygon[i];
                Vector2 b = polygon[j];
                if (point.y < a.y != point.y < b.y)
                {
                    float x = (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x;

                    if (point.x < x)
                    {
                        outside = !outside;
                    }
                }
            }

            if (outside)
                return 1;
            else
                return -1;
        }

        //public bool Overlap(Polygon2 other, out float ratio)
        //{
        //    return Overlap(points, other.points, out ratio);
        //}

        //public static bool Overlap(List<Vector2> origin, List<Vector2> other, out float ratio)
        //{
        //    ratio = Mathf.Abs(Polygon2.CalculateArea(origin) / Polygon2.CalculateArea(other));

        //    bool flag = true;
        //    int insideCount = 0;
        //    if (ratio <= 1)
        //    {
        //        foreach (var point in origin)
        //        {
        //            int side = Polygon2.Side(point, other);
        //            if (side == 1)
        //            {
        //                flag = false;
        //                break;
        //            }
        //            else if (side == 0)
        //            {
        //                insideCount++;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (var point in other)
        //        {
        //            int side = Polygon2.Side(point, origin);
        //            if (side == 1)
        //            {
        //                flag = false;
        //                break;
        //            }
        //            else if (side == 0)
        //            {
        //                insideCount++;
        //            }
        //        }
        //    }

        //    if (insideCount < 2)
        //    {
        //        flag = false;
        //    }

        //    return flag;
        //}

        /// <summary>
        /// 两个多边形是否相交的情况
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public bool Intersects(Polygon2 other)
        {
            Rect rect0 = MathUtility.GetRect(this.points);
            Rect rect1 = MathUtility.GetRect(other.points);

            if (!rect0.Overlaps(rect1))
                return false;

            // 边相交。2个多边形的边是否相交。
            List<Segment2> segments0 = GeometryUtils.ToSegmets(this.points);
            List<Segment2> segments1 = GeometryUtils.ToSegmets(other.points);
            foreach (var segment0 in segments0)
            {
                foreach (var segment1 in segments1)
                {
                    if (segment0.Intersect(segment1, out Vector2 point))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 多边形是否包含 other 多边形
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Contains(Polygon2 other)
        {
            foreach (var point in other.points)
            {
                if (!this.Inside(point))
                    return false;
            }

            if (Intersects(other))
                return false;

            return true;
        }

        /// <summary>
        /// 两多边形是否有重叠部分
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(Polygon2 other)
        {
            if (Intersects(other))
            {
                return true;
            }

            foreach (var point in other.points)
            {
                if (this.Inside(point))
                    return true;
            }

            foreach (var point in this.points)
            {
                if (other.Inside(point))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Polygon2 other)
        {
            return points.Equals(other.points);
        }
    }
}
