using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class Corner : EntityObject
    {
        /// <summary>
        /// 元件类型
        /// </summary>
        public override EntityType Type
        {
            get
            {
                return EntityType.Corner;
            }
        }

        private Vector3 position;
        /// <summary>
        /// 墙角的位置
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set
            {
                SetProperty(ref position, value, Constants.position);
            }
        }

        private ObservableCollection<Wall> m_Walls = new ObservableCollection<Wall>();
        /// <summary>
        /// 连接的墙列表
        /// </summary>
        public ObservableCollection<Wall> Walls
        {
            get { return m_Walls; }
        }

        private List<int> wallOrders = new List<int>();
        /// <summary>
        /// 墙体的序号列表
        /// </summary>
        public List<int> WallOrders
        {
            get { return wallOrders; }
        }

        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);
            switch (propertyName)
            {
                case Constants.position:
                    OnTransformChanged();
                    break;
                default:
                    break;
            }
        }

        public Corner()
        {
            Special = "建筑";
        }

        /// <summary>
        /// 添加墙体
        /// </summary>
        /// <param name="wall"></param>
        public void AddWall(Wall wall)
        {
            if (m_Walls.Contains(wall))
                return;

            m_Walls.Add(wall);
        }

        /// <summary>
        /// 移除墙体
        /// </summary>
        /// <param name="wall"></param>
        public void RemoveWall(Wall wall)
        {
            m_Walls.Remove(wall);
        }

        /// <summary>
        /// 根据目标墙，获取逆时针/顺时针方向的墙
        /// </summary>
        /// <param name="target"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Wall GetWall(Wall target, ClockDirection direction = ClockDirection.CounterClockwise)
        {
            Wall wall = null;
            if (m_Walls.Count <= 1)
            {
                return wall;
            }

            int wall_index = m_Walls.IndexOf(target);
            if (wall_index == -1)
            {
                return wall;
            }

            int index = wallOrders.IndexOf(wall_index);
            switch (direction)
            {
                case ClockDirection.Clockwise:
                    index = (index == 0) ? m_Walls.Count - 1 : index - 1;
                    break;
                case ClockDirection.CounterClockwise:
                    index = (index == m_Walls.Count - 1) ? 0 : index + 1;
                    break;
                default:
                    break;
            }

            wall_index = wallOrders[index];
            wall = m_Walls[wall_index];
            return wall;
        }

        /// <summary>
        /// 同步墙体，逆时针排序的序号
        /// </summary>
        public void Sync()
        {
            List<Vector2> vectors = ListPool<Vector2>.Get();

            foreach (var item in m_Walls)
            {
                Vector2 vector = item.ToVector2(this);
                vectors.Add(vector);
            }
            MathUtility.CounterClockwise(vectors, ref wallOrders);

            ListPool<Vector2>.Release(vectors);
        }

        /// <summary>
        /// 获取连接点的列表
        /// </summary>
        /// <returns></returns>
        public List<Corner> GetConnectedCorners()
        {
            List<Corner> corners = new List<Corner>();
            foreach (var wall in Walls)
            {
                corners.Add(wall.Another(this));
            }
            return corners;
        }

        public override object Clone()
        {
            Corner entity = new Corner
            {
                position = this.position,
            };

            return entity;
        }
    }
}
