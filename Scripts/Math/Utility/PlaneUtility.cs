using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    public static class PlaneUtility
    {
        /// <summary>
        /// Matrix that transforms a point from local space into world space.
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Matrix4x4 LocalToWorldMatrix(this Plane plane)
        {
            Vector3 position = plane.ClosestPointOnPlane(Vector3.zero);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, plane.normal);
            return Matrix4x4.TRS(position, rotation, Vector3.one);
        }

        /// <summary>
        /// Matrix that transforms a point from world space into local space.
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Matrix4x4 WorldToLocalMatrix(this Plane plane)
        {
            return plane.LocalToWorldMatrix().inverse;
        }

        /// <summary>
        /// Transforms position from world space to local space.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 InverseTransformPoint(this Plane plane, Vector3 position)
        {
            return plane.WorldToLocalMatrix().MultiplyPoint(position);
        }

        /// <summary>
        /// Transforms position from local space to world space.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 TransformPoint(this Plane plane, Vector3 position)
        {
            return plane.LocalToWorldMatrix().MultiplyPoint(position);
        }

        /// <summary>
        /// The rotation of the plane int world space.
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Quaternion Rotation(this Plane plane)
        {
            return Quaternion.FromToRotation(Vector3.up, plane.normal);
        }

        /// <summary>
        /// The red axis of the plane in world space.
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Vector3 Right(this Plane plane)
        {
            return Quaternion.FromToRotation(Vector3.up, plane.normal) * Vector3.right;
        }

        /// <summary>
        /// The green axis of plane in world space.
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Vector3 Up(this Plane plane)
        {
            return Quaternion.FromToRotation(Vector3.up, plane.normal) * Vector3.up;
        }

        /// <summary>
        /// The blue axis of the plane in world space.
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Vector3 Forward(this Plane plane)
        {
            return Quaternion.FromToRotation(Vector3.up, plane.normal) * Vector3.forward;
        }
    }
}
