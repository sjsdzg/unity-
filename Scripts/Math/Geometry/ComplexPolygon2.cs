using LibTessDotNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Math
{
    /// <summary>
    /// 复杂多边形 
    /// 1.含有多个岛洞的多边形
    /// 2.彼此不存在嵌套岛洞的情形
    /// </summary>
    public struct ComplexPolygon2 : IEquatable<Polygon2>
    {
        /// <summary>
        /// 轮廓列表
        /// 轮廓：逆时针排列
        /// 岛洞：顺时针排列
        /// </summary>
        public List<List<Vector2>> contours;

        public ComplexPolygon2(List<List<Vector2>> contours)
        {
            this.contours = contours;
        }

        /// <summary>
        /// 外轮廓
        /// </summary>
        public List<Vector2> OuterContour
        {
            get
            {
                if (contours == null || contours.Count == 0)
                    return null;

                return contours[0];
            }
        }

        /// <summary>
        /// 获取视心（用于在多边形上优化文本标签的位置。）
        /// </summary>
        /// <returns></returns>
        public Vector2 GetVisualPoint(float precision)
        {
            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();

            Rect rect = Polygon2.GetBounds(OuterContour);
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
            Vector2 centroid = Polygon2.GetCentroid(OuterContour);
            Cell bestCell = new Cell(centroid, 0, this);

            // special case for rectangular polygons
            Cell boxCell = new Cell(rect.center, 0, this);
            //if (boxCell.distance > bestCell.distance)
            //    bestCell = boxCell;
            if (MathUtility.Greater(boxCell.distance, bestCell.distance))
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
                //if (cell.distance > bestCell.distance)
                if (MathUtility.Greater(cell.distance, bestCell.distance))
                {
                    bestCell = cell;
                    //Debug.Log("bestCell -- " + bestCell.ToString());
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
        /// 点是否在多边形内部
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public bool Inside(Vector2 point)
        {
            return Inside(point, contours);
        }

        /// <summary>
        /// 点是否在多边形外部
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public bool Outside(Vector2 point)
        {
            return Outside(point, contours);
        }


        /// <summary>
        /// 点是否在多边形上
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public bool Onside(Vector2 point)
        {
            return Onside(point, contours);
        }

        /// <summary>
        /// signed distance from point to polygon outline
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetDistanceToPoint(Vector2 point)
        {
            return GetDistanceToPoint(point, contours);
        }

        /// <summary>
        /// signed distance from point to polygon outline
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetSignDistanceToPoint(Vector2 point)
        {
            return GetSignDistanceToPoint(point, contours);
        }

        /// <summary>
        /// signed distance from point to polygon outline (negative if point is outside)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float GetDistanceToPoint(Vector2 point, List<List<Vector2>> polygon)
        {
            float distance = float.MaxValue;
            foreach (var contour in polygon)
            {
                for (int i = 0, length = contour.Count, j = length - 1; i < length; j = i++)
                {
                    Vector2 a = contour[i];
                    Vector2 b = contour[j];
                    Segment2 segment = new Segment2(a, b);
                    distance = Mathf.Min(distance, segment.GetDistanceToPoint(point));
                }
            }
            return distance;
        }

        /// <summary>F:\Projects\SSIT-01-2019\Assets\Scripts\Architecture\Entities\Group.cs
        /// signed distance from point to polygon outline (negative if point is outside)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float GetSignDistanceToPoint(Vector2 point, List<List<Vector2>> polygon)
        {
            float distance = float.MaxValue;
            bool inside = false;

            foreach (var contour in polygon)
            {
                for (int i = 0, length = contour.Count, j = length - 1; i < length; j = i++)
                {
                    Vector2 a = contour[i];
                    Vector2 b = contour[j];
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
            }

            return (inside ? 1 : -1) * distance;
        }

        /// <summary>
        /// 点是否在多边形内部
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool Inside(Vector2 point, List<List<Vector2>> polygon)
        {
            if (Onside(point, polygon))
            {
                return false;
            }

            bool inside = false;
            foreach (var contour in polygon)
            {
                int length = contour.Count;
                if (length < 3)
                    throw new ArgumentException("contour of polygon can not have less than 3 point");

                for (int i = 0, j = length - 1; i < length; j = i++)
                {
                    Vector2 a = contour[i];
                    Vector2 b = contour[j];
                    if (point.y < a.y != point.y < b.y)
                    {
                        float x = (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x;

                        if (point.x < x)
                        {
                            inside = !inside;
                        }
                    }
                }
            }

            return inside;
        }

        /// <summary>
        /// 点是否在多边形上
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool Onside(Vector2 point, List<List<Vector2>> polygon)
        {
            float distance = GetDistanceToPoint(point, polygon);
            if (distance == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 点是否在多边形内部
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool Outside(Vector2 point, List<List<Vector2>> polygon)
        {
            if (Onside(point, polygon))
            {
                return false;
            }

            bool outside = true;
            foreach (var contour in polygon)
            {
                int length = contour.Count;
                if (length < 3)
                    throw new ArgumentException("contour of polygon can not have less than 3 point");

                for (int i = 0, j = length - 1; i < length; j = i++)
                {
                    Vector2 a = contour[i];
                    Vector2 b = contour[j];
                    if (point.y < a.y != point.y < b.y)
                    {
                        float x = (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x;

                        if (point.x < x)
                        {
                            outside = !outside;
                        }
                    }
                }
            }

            return outside;
        }

        /// <summary>
        /// 点和多边形的关系
        /// 1 ：点在多边形外部
        /// 0 ：点在多边形的边上
        /// -1：点在多边形内部
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static int Side(Vector2 point, List<List<Vector2>> polygon)
        {
            if (Onside(point, polygon))
            {
                return 0;
            }

            bool outside = true;
            foreach (var contour in polygon)
            {
                int length = contour.Count;
                if (length < 3)
                    throw new ArgumentException("contour of polygon can not have less than 3 point");

                for (int i = 0, j = length - 1; i < length; j = i++)
                {
                    Vector2 a = contour[i];
                    Vector2 b = contour[j];
                    if (point.y < a.y != point.y < b.y)
                    {
                        float x = (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x;

                        if (point.x < x)
                        {
                            outside = !outside;
                        }
                    }
                }
            }


            if (outside)
                return 1;
            else
                return -1;
        }


        /// <summary>
        /// 梁多边形是否有相交部分
        /// </summary>
        /// <param name="points0"></param>
        /// <param name="points1"></param>
        /// <returns></returns>
        public bool Intersects(ComplexPolygon2 other)
        {
            foreach (var contour in contours)
            {
                foreach (var otherContour in other.contours)
                {
                    Polygon2 originPolygon = new Polygon2(contour);
                    Polygon2 otherPolygon = new Polygon2(otherContour);
                    if (originPolygon.Intersects(otherPolygon))
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
        public bool Contains(ComplexPolygon2 other)
        {
            foreach (var contour in other.contours)
            {
                foreach (var point in contour)
                {
                    if (!this.Inside(point))
                        return false;
                }
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
        public bool Overlaps(ComplexPolygon2 other)
        {
            if (Intersects(other))
            {
                return true;
            }

            foreach (var contour in other.contours)
            {
                foreach (var point in contour)
                {
                    if (!this.Inside(point))
                        return false;
                }
            }


            foreach (var contour in this.contours)
            {
                foreach (var point in contour)
                {
                    if (!other.Inside(point))
                        return false;
                }
            }


            return false;
        }

        public bool Equals(Polygon2 other)
        {
            return this.Equals(other);
        }

    }

    /// <summary>
    /// 单元格
    /// </summary>
    public struct Cell
    {
        /// <summary>
        /// cell center
        /// </summary>
        public readonly Vector2 center;

        /// <summary>
        /// half the cell size
        /// </summary>
        public readonly float half;

        /// <summary>
        /// distance from cell center to polygon
        /// </summary>
        public readonly float distance;

        /// <summary>
        /// max distance to polygon within a cell
        /// </summary>
        public readonly float maxDistance;

        public Cell(Vector2 center, float half, ComplexPolygon2 polygon)
        {
            this.center = center;
            this.half = half;
            this.distance = polygon.GetSignDistanceToPoint(center);
            this.maxDistance = this.distance + this.half * Mathf.Sqrt(2);
        }

        public Cell(float x, float y, float half, ComplexPolygon2 polygon)
        {
            this.center = new Vector2(x, y);
            this.half = half;
            this.distance = polygon.GetSignDistanceToPoint(center);
            this.maxDistance = this.distance + this.half * Mathf.Sqrt(2);
        }

        public Cell(Vector2 center, float half, Polygon2 polygon)
        {
            this.center = center;
            this.half = half;
            this.distance = polygon.GetSignDistanceToPoint(center);
            this.maxDistance = this.distance + this.half * Mathf.Sqrt(2);
        }

        public Cell(float x, float y, float half, Polygon2 polygon)
        {
            this.center = new Vector2(x, y);
            this.half = half;
            this.distance = polygon.GetSignDistanceToPoint(center);
            this.maxDistance = this.distance + this.half * Mathf.Sqrt(2);
        }

        public override string ToString()
        {
            return "center: " + center + "\n" +
                "half: " + half + "\n" +
                "distance: " + distance + "\n" +
                "maxDistance: " + maxDistance + "\n";
        }
    }

    public class CellComparator : IComparer<Cell>
    {
        public int Compare(Cell x, Cell y)
        {
            return y.maxDistance.CompareTo(x.maxDistance);
        }
    }
}
