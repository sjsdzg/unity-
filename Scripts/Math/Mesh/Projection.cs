using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    /// <summary>
    /// Axis used in projecting UVs.
    /// </summary>
    public enum ProjectionAxis
    {
        /// <summary>
        /// Projects on x axis.
        /// </summary>
        X,
        /// <summary>
        /// Projects on y axis.
        /// </summary>
        Y,
        /// <summary>
        /// Projects on z axis.
        /// </summary>
        Z,
        /// <summary>
        /// Projects on -x axis.
        /// </summary>
        XNegative,
        /// <summary>
        /// Projects on -y axis.
        /// </summary>
        YNegative,
        /// <summary>
        /// Projects on -z axis.
        /// </summary>
        ZNegative,
    }

    public static class Projection
    {
        /// <summary>
        /// Find a plane that best fits a set of 3d points.
        /// </summary>
        /// <remarks>http://www.ilikebigbits.com/blog/2015/3/2/plane-from-points</remarks>
        /// <param name="points">The points to find a plane for. Order does not matter.</param>
        /// <param name="indices">If provided, only the vertices by the indices array will be considered.</param>
        /// <returns>A plane that best matchs the layout of the points array.</returns>
        public static Plane FindBestPlane(IList<Vector3> points, IList<int> indices = null)
        {
            float xx = 0f, xy = 0f, xz = 0f,
                  yy = 0f, yz = 0f, zz = 0f;

            if (points == null)
                throw new System.ArgumentNullException("points");

            bool flag = indices != null && indices.Count > 0;
            int length = flag ? indices.Count : points.Count;

            Vector3 center = Vector3.zero, normal = Vector3.zero;

            for (int i = 0; i < length; i++)
            {
                center.x += points[flag ? indices[i] : i].x;
                center.y += points[flag ? indices[i] : i].y;
                center.z += points[flag ? indices[i] : i].z;
            }

            center.x /= (float)length;
            center.y /= (float)length;
            center.z /= (float)length;

            for (int i = 0; i < length; i++)
            {
                Vector3 r = points[flag ? indices[i] : i] - center;

                xx += r.x * r.x;
                xy += r.x * r.y;
                xz += r.x * r.z;
                yy += r.y * r.y;
                yz += r.y * r.z;
                zz += r.z * r.z;
            }

            float det_x = yy * zz - yz * yz;
            float det_y = xx * zz - xz * xz;
            float det_z = xx * yy - xy * xy;

            if (det_x > det_y && det_x > det_z)
            {
                normal.x = 1f;
                normal.y = (xz * yz - xy * zz) / det_x;
                normal.z = (xy * yz - xz * yy) / det_x;
            }
            else if (det_y > det_z)
            {
                normal.x = (yz * xz - xy * zz) / det_y;
                normal.y = 1f;
                normal.z = (xy * xz - yz * xx) / det_y;
            }
            else
            {
                normal.x = (yz * xy - xz * yy) / det_z;
                normal.y = (xz * xy - yz * xx) / det_z;
                normal.z = 1f;
            }

            normal.Normalize();

            return new Plane(normal, center);
        }

        public static Vector3 GetTangentToAxis(ProjectionAxis axis)
        {
            switch (axis)
            {
                case ProjectionAxis.X:
                case ProjectionAxis.XNegative:
                    return Vector3.up;

                case ProjectionAxis.Y:
                case ProjectionAxis.YNegative:
                    return Vector3.forward;

                case ProjectionAxis.Z:
                case ProjectionAxis.ZNegative:
                    return Vector3.up;

                default:
                    return Vector3.up;
            }
        }

        /// <summary>
        /// Returs a projection axis based on axis is the largest.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static ProjectionAxis VectorToProjectionAxis(Vector3 direction)
        {
            float x = Mathf.Abs(direction.x);
            float y = Mathf.Abs(direction.y);
            float z = Mathf.Abs(direction.z);

            if (!Mathf.Approximately(x, y) && x > y && !Mathf.Approximately(x, z) && x > z)
                return direction.x > 0 ? ProjectionAxis.X : ProjectionAxis.XNegative;

            if (!Mathf.Approximately(y, z) && y > z)
                return direction.y > 0 ? ProjectionAxis.Y : ProjectionAxis.YNegative;

            return direction.z > 0 ? ProjectionAxis.Z : ProjectionAxis.ZNegative;
        }

        /// <summary>
        /// Given a ProjectionAxis, return the appropriate Vector3 conversion.
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static Vector3 ProjectionAxisToVertor(ProjectionAxis axis)
        {
            switch (axis)
            {
                case ProjectionAxis.X:
                    return Vector3.right;

                case ProjectionAxis.Y:
                    return Vector3.up;

                case ProjectionAxis.Z:
                    return Vector3.forward;

                case ProjectionAxis.XNegative:
                    return -Vector3.right;

                case ProjectionAxis.YNegative:
                    return -Vector3.up;

                case ProjectionAxis.ZNegative:
                    return -Vector3.forward;

                default:
                    return Vector3.zero;
            }
        }

        /// <summary>
        /// Project a collection of 3d positions to a 2d plane. The direction from the vertices are projected.
        /// </summary>
        /// <param name="positions">A collection of positions to project based on a direction.</param>
        /// <param name="indices"></param>
        /// <returns>The positions array projected into 2d coordinates.</returns>
        public static Vector2[] PlanerProject(IList<Vector3> positions, IList<int> indices = null)
        {
            return PlanerProject(positions, indices, FindBestPlane(positions, indices).normal);
        }

        /// <summary>
        /// Project a collection of 3d positions to a 2d plane.
        /// </summary>
        /// <param name="positions">A collection of positions to project based on a direction.</param>
        /// <param name="indices"></param>
        /// <param name="direction">The direction from which vertex positions are projected into 2d space.</param>
        /// <returns>The positions array projected into 2d coordinates.</returns>
        public static Vector2[] PlanerProject(IList<Vector3> positions, IList<int> indices, Vector3 direction)
        {
            List<Vector2> results = new List<Vector2>(indices != null ? indices.Count : positions.Count);
            PlanerProject(positions, indices, direction, results);
            return results.ToArray();
        }

        /// <summary>
        /// Project a collection of 3d positions to a 2d plane.
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="indices"></param>
        /// <param name="direction"></param>
        /// <param name="results"></param>
        public static void PlanerProject(IList<Vector3> positions, IList<int> indices, Vector3 direction, List<Vector2> results)
        {
            if (positions == null)
                throw new ArgumentNullException("positions");

            if (results == null)
                throw new ArgumentNullException("results");

            var normal = direction;
            var axis = VectorToProjectionAxis(normal);
            var tangent = GetTangentToAxis(axis);
            var length = indices == null ? positions.Count : indices.Count;
            results.Clear();

            var u = Vector3.Cross(normal, tangent);
            var v = Vector3.Cross(u, normal);

            u.Normalize();
            v.Normalize();

            if (indices != null)
            {
                for (int i = 0; i < length; i++)
                    results.Add(new Vector2(Vector3.Dot(u, positions[indices[i]]), Vector3.Dot(v, positions[indices[i]])));
            }
            else
            {
                for (int i = 0; i < length; i++)
                    results.Add(new Vector2(Vector3.Dot(u, positions[i]), Vector3.Dot(v, positions[i])));
            }
        }
    }
}
