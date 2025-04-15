using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    public struct Sphere : IEquatable<Sphere>
    {
        /// <summary>
        /// 中心
        /// </summary>
        public Vector3 center;

        /// <summary>
        /// 半径
        /// </summary>
        public float radius;

        public Sphere(float radius)
        {
            this.center = Vector3.zero;
            this.radius = radius;
        }

        public Sphere(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        /// <summary>
        /// 根据水平旋转角度和竖直旋转角度，获取球上的点坐标
        /// </summary>
        /// <param name="horizontalAngle"></param>
        /// <param name="verticalAngle"></param>
        /// <returns></returns>
        public Vector3 GetPoint(float horizontalAngle, float verticalAngle)
        {
            float x = radius * Mathf.Cos(verticalAngle) * Mathf.Cos(horizontalAngle);
            float y = radius * Mathf.Sin(verticalAngle);
            float z = radius * Mathf.Cos(verticalAngle) * Mathf.Sin(horizontalAngle);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// 点是否在球内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Vector3 point)
        {
            return (point - center).magnitude < radius;
        }

        public bool Equals(Sphere other)
        {
            return this.center.Equals(other.center) && this.radius.Equals(other.radius);
        }

        public override string ToString()
        {
            return string.Format("Sphere(center: {0}, radius: {1})", center, radius);
        }
    }
}
