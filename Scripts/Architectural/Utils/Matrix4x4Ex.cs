﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Architectural
{
    public static class Matrix4x4Ex
    {
        public static Vector3 GetPosition(this Matrix4x4 matrix)
        {
            var x = matrix.m03;
            var y = matrix.m13;
            var z = matrix.m23;

            return new Vector3(x, y, z);
        }

        public static Quaternion GetRotation(this Matrix4x4 matrix)
        {
            float qw = Mathf.Sqrt(1f + matrix.m00 + matrix.m11 + matrix.m22) / 2;
            float w = 4 * qw;
            float qx = (matrix.m21 - matrix.m12) / w;
            float qy = (matrix.m02 - matrix.m20) / w;
            float qz = (matrix.m10 - matrix.m01) / w;

            return new Quaternion(qx, qy, qz, qw);
        }

        public static Vector3 GetScale(this Matrix4x4 matrix)
        {
            var x = Mathf.Sqrt(matrix.m00 * matrix.m00 + matrix.m01 * matrix.m01 + matrix.m02 * matrix.m02);
            var y = Mathf.Sqrt(matrix.m10 * matrix.m10 + matrix.m11 * matrix.m11 + matrix.m12 * matrix.m12);
            var z = Mathf.Sqrt(matrix.m20 * matrix.m20 + matrix.m21 * matrix.m21 + matrix.m22 * matrix.m22);

            return new Vector3(x, y, z);
        }
    }
}
