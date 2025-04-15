using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Math
{
    /// <summary>
    /// 样条曲线
    /// </summary>
    public class Spline
    {
        public enum Type { Linear, Bezier, BSpline, CatmullRom}

        /// <summary>
        /// 是否闭合
        /// </summary>
        public bool closed = false;

        /// <summary>
        /// 样条曲线类型
        /// </summary>
        public Type type = Type.Linear;

        /// <summary>
        /// 样条点集合
        /// </summary>
        public List<SplinePoint> points = new List<SplinePoint>();

        public void AddPoint()
        {

        }

        public void RemovePoint()
        {

        }
    }
}
