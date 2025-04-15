using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class WallCastHit
    {
        /// <summary>
        /// 墙体
        /// </summary>
        public Wall Wall { get; set; }

        /// <summary>
        /// 交点
        /// </summary>
        public Vector2 Point { get; set; }
    }

    public static class WallHandler
    {
        /// <summary>
        /// 添加 Wall
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="wall"></param>
        public static void AddWall(Floor floor, Wall target)
        {

        }

        /// <summary>
        /// 移除 Wall
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="wall"></param>
        public static void RemoveWall(Floor floor, Wall target)
        {

        }

        /// <summary>
        /// 沿着墙体的线段投射，返回相交的墙体信息
        /// </summary>
        /// <param name="target"></param>
        /// <param name="walls"></param>
        /// <returns></returns>
        public static List<WallCastHit> WallCast(Wall target, List<Wall> walls)
        {
            List<WallCastHit> hits = new List<WallCastHit>();
            Segment2 castSegment = target.ToSegment2(target.Corner0);
            // 和其他墙体 求交
            Segment2 segment = new Segment2();
            foreach (var wall in walls)
            {
                segment.p1 = wall.Corner0.ToPoint2();
                segment.p2 = wall.Corner1.ToPoint2();
                if (castSegment.Intersect(segment, out Vector2 point))
                {
                    WallCastHit hit = new WallCastHit();
                    hit.Wall = wall;
                    hit.Point = point;
                    hits.Add(hit);
                }
            }

            return hits;
        }
    }
}
