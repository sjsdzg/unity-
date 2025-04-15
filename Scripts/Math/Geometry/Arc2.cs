using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    /// <summary>
    /// 圆弧
    /// 默认逆时针
    /// </summary>
    public struct Arc2 : IEquatable<Arc2>
    {
        /// <summary>
        /// 中心点
        /// </summary>
        public Vector2 center;

        /// <summary>
        /// 半径
        /// </summary>
        public float radius;

        /// <summary>
        /// 起始角（弧度）
        /// </summary>
        public float startAngle;

        /// <summary>
        /// 扫描角（弧度）
        /// </summary>
        public float sweepAngle;

        /// <summary>
        /// 取值 [0, 1]
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector2 SampleT(float t)
        {
            float theta = startAngle + t * sweepAngle;
            float x = Mathf.Cos(theta), y = Mathf.Sin(theta);
            return new Vector2(x, y);
        }

        /// <summary>
        /// 切线 [0, 1]
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector2 TangentT(float t)
        {
            float theta = startAngle + t * sweepAngle;
            float x = Mathf.Cos(theta), y = Mathf.Sin(theta);
            Vector2 tangent = new Vector2(-y, x);
            tangent.Normalize();
            return tangent;
        }

        public float GetDistanceToPoint(Vector2 point)
        {
            Vector2 vector = point - center;
            float length = vector.magnitude;
            if (MathUtility.Appr(length, 0))
            {
                return radius;
            }
            else
            {
                Vector2 dv = vector / length;
                float theta = Mathf.Atan2(dv.y, dv.x);

                if (theta >= startAngle && theta <= startAngle + sweepAngle)
                {
                    return System.Math.Abs(length - radius);
                }
                else
                {
                    float ctheta = MathUtility.ClampAngleRad(theta, startAngle, startAngle + sweepAngle);
                    Vector2 pos = new Vector2(center.x + radius * Mathf.Cos(ctheta), center.y + radius * Mathf.Sin(ctheta));
                    return Vector2.Distance(pos, point);
                }
            }
        }



        public bool Equals(Arc2 other)
        {
            return this.center.Equals(other.center)
                && this.radius.Equals(other.radius)
                && this.startAngle.Equals(other.startAngle)
                && this.sweepAngle.Equals(other.sweepAngle);
        }

        public override string ToString()
        {
            return string.Format("Circle2(center: {0}, radius: {1}, startAngle: {2}, sweepAngle: {3})", center, radius, startAngle, sweepAngle);
        }
    }
}
