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
    /// <summary>
    /// 区域
    /// </summary>
    public class Area : EntityObject
    {
        public override EntityType Type
        {
            get { return EntityType.Area; }
        }


        private List<Vector3> m_Contour = new List<Vector3>();
        /// <summary>
        /// 轮廓
        /// </summary>
        public List<Vector3> Contour
        {
            get { return m_Contour; }
            set { m_Contour = value; }
        }

        /// <summary>
        /// 获取视心
        /// </summary>
        /// <returns></returns>
        public Vector3 GetVisualPoint()
        {
            List<Vector2> points = ListPool<Vector2>.Get();
            foreach (var point in m_Contour)
            {
                points.Add(point.XZ());
            }
            Polygon2 polygon = new Polygon2(points);
            Vector2 visualPoint = polygon.GetVisualPoint(0.5f);
            return visualPoint.XOZ(m_Contour[0].y);
        }

        public Area()
        {
            Special = "建筑";
            Name = "未命名";
        }

        public void SetContour(List<Vector3> points)
        {
            m_Contour.AddRange(points);
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
