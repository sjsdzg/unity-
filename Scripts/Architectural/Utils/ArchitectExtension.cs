using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Math;
using XFramework.Architectural;
using XFramework.Core;

namespace XFramework.Architectural
{
    public static class ArchitectExtension
    {
        public static Vector2 ToPoint2(this Corner corner)
        {
            return new Vector2(corner.Position.x, corner.Position.z);
        }

        public static Line2 ToLine2(this Wall wall, Corner corner)
        {
            if (corner == wall.Corner0)
            {
                return new Line2(wall.Corner0.ToPoint2(), wall.Corner1.ToPoint2());
            }
            else
            {
                return new Line2(wall.Corner1.ToPoint2(), wall.Corner0.ToPoint2());
            }
        }

        public static Segment2 ToSegment2(this Wall wall, Corner corner)
        {
            if (corner == wall.Corner0)
            {
                return new Segment2(wall.Corner0.ToPoint2(), wall.Corner1.ToPoint2());
            }
            else
            {
                return new Segment2(wall.Corner1.ToPoint2(), wall.Corner0.ToPoint2());
            }
        }

        public static Vector2 ToVector2(this Wall wall, Corner corner)
        {
            if (wall.Corner0 == corner)
            {
                return wall.Corner1.ToPoint2() - wall.Corner0.ToPoint2();
            }
            else
            {
                return wall.Corner0.ToPoint2() - wall.Corner1.ToPoint2();
            }
        }

        /// <summary>
        /// 获取另一个
        /// </summary>
        /// <param name="corner"></param>
        /// <returns></returns>
        public static Corner Another(this Wall wall, Corner corner)
        {
            if (corner == wall.Corner0)
            {
                return wall.Corner1;
            }
            else
            {
                return wall.Corner0;
            }
        }

        /// <summary>
        /// 是否同向
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool SameDirection(this Wall wall, Wall other)
        {
            if (wall.Corner0 == other.Corner1 || wall.Corner1 == other.Corner0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Line2 GetLine2(this Wall wall, Corner corner, ClockDirection direction = ClockDirection.CounterClockwise)
        {
            Line2 line = wall.ToLine2(corner);
            switch (direction)
            {
                case ClockDirection.Clockwise:
                    line = line.Move(-wall.Thickness / 2);
                    break;
                case ClockDirection.CounterClockwise:
                    line = line.Move(wall.Thickness / 2);
                    break;
                default:
                    break;
            }
            return line;
        }

        /// <summary>
        /// 是否存在墙角点重合
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool OverlapCorner(this Wall wall, Wall other)
        {
            return (wall.Corner0 == other.Corner0
                || wall.Corner0 == other.Corner1
                || wall.Corner1 == other.Corner0
                || wall.Corner1 == other.Corner1);
        }


        public static Vector2 ToPoint2(this WallHole wallHole)
        {
            return new Vector2(wallHole.Position.x, wallHole.Position.z);
        }

        public static Vector2 ToVector2(this WallHole wallHole)
        {
            return wallHole.Wall.ToVector2(wallHole.Wall.Corner0).normalized;
        }
    }
}

