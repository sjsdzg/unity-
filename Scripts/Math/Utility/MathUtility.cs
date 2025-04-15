using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    public struct IndexFloat : IComparable<IndexFloat>
    {
        public int index;
        public float value;

        public int CompareTo(IndexFloat other)
        {
            return this.value.CompareTo(other.value);
        }
    }

    public static class MathUtility
    {
        /// <summary>
        ///  A tiny floating point value (Read Only).
        /// </summary>
        public const float Epsilon = 1E-04F;

        public const float TwoPI = 2 * Mathf.PI;

        /// <summary>
        /// 近似相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool Appr(float a, float b)
        {
            if (Mathf.Abs(a - b) <= Epsilon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 近似相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool Appr(Vector2 a, Vector2 b)
        {
            if (Appr(a.x, b.x) && Appr(a.y, b.y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 近似相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool Appr(Vector3 a, Vector3 b)
        {
            if (Appr(a.x, b.x) && Appr(a.y, b.y) && Appr(a.z, b.z))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Greater(float a, float b)
        {
            if (a - b > Epsilon)
                return true;
            else
                return false;
        }

        public static bool Less(float a, float b)
        {
            if (b - a > Epsilon)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 将Vector3中的y,z转换成Vector2
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 YZ(this Vector3 v)
        {
            return new Vector2(v.y, v.z);
        }

        /// <summary>
        /// 将Vector3中的x,z转换成Vector2
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 XZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }

        /// <summary>
        /// 将Vector2中的x,y 转换成Vector3中的x,z
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 XOZ(this Vector3 v, float y = 0)
        {
            return new Vector3(v.x, y, v.y);
        }

        /// <summary>
        /// 将Vector2中的x,y 转换成Vector3中的x,z
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 XOZ(this Vector2 v, float y = 0)
        {
            return new Vector3(v.x, y, v.y);
        }

        /// <summary>
        /// 将Vector2中的x,y 转换成Vector3中的y,z
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 OYZ(this Vector2 v, float x = 0)
        {
            return new Vector3(x, v.x, v.y);
        }

        /// <summary>
        /// 2维向量的叉乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        /// <summary>
        /// 向量 是否平行
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsParallel(Vector2 a, Vector2 b)
        {
            float f = Cross(a, b);
            if (Appr(f, 0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 计算面积
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static float CalculateArea(List<Vector3> positions, Vector3 normal)
        {
            Vector2[] points = Projection.PlanerProject(positions, null, normal);
            float area = Polygon2.CalculateArea(points.ToList());
            return Mathf.Abs(area);
        }

        /// <summary>
        /// 一个向量在逆时针排序的向量列表中位置
        /// </summary>
        /// <param name="a"></param>
        /// <param name="vectors"></param>
        /// <returns></returns>
        public static int Where(Vector2 a, List<Vector2> vectors)
        {
            int index = 0;

            if (vectors == null)
            {
                index = -1;
            }
            else if (vectors.Count == 0)
            {
                index = 0;
            }
            else
            {
                // 归一化
                for (int i = 0; i < vectors.Count; i++)
                {
                    vectors[i] = vectors[i].normalized;
                }
                a = a.normalized;
                // 找出最右边的点，即x最小的点。
                Vector2 origin = new Vector2(1, 0);
                // 依次求夹角，从小到大排列，即按逆时针排列
                float k0 = Vector2.Angle((a - origin), Vector2.up);
                float k1;
                for (int i = 0; i < vectors.Count; i++)
                {
                    k1 = Vector2.Angle((vectors[i] - origin), Vector2.up);
                    if (k0 > k1)
                    {
                        index++;
                    }
                }
            }

            return index;
        }


        /// <summary>
        /// 一组向量逆时针排序，获取对应向量的序号
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns></returns>
        public static void CounterClockwise(List<Vector2> vectors, ref List<int> indices)
        {
            int count = vectors.Count;
            // 归一化
            for (int i = 0; i < count; i++)
            {
                vectors[i] = vectors[i].normalized;
            }

            IndexFloat[] array = new IndexFloat[count];
            Vector2 origin = new Vector2(1, 0);
            for (int i = 0; i < count; i++)
            {
                array[i].index = i;
                array[i].value = Vector2.Angle(vectors[i] - origin, Vector2.up);
            }
            Array.Sort(array);

            indices.Clear();
            for (int i = 0; i < count; i++)
            {
                indices.Add(array[i].index);

            }
        }


        /// <summary>
        /// Average for a set of 2d array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public static Vector3 Average(IList<Vector2> array, IList<int> indices = null)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            Vector3 sum = Vector3.zero;

            bool flag = indices != null && indices.Count > 0;
            int length = flag ? indices.Count : array.Count;

            for (int i = 0; i < length; i++)
            {
                sum.x += array[flag ? indices[i] : i].x;
                sum.y += array[flag ? indices[i] : i].y;
            }

            return sum / length;
        }

        /// <summary>
        /// Average for a set of 3d array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public static Vector3 Average(IList<Vector3> array, IList<int> indices = null)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            Vector3 sum = Vector3.zero;

            bool flag = indices != null && indices.Count > 0;
            int length = flag ? indices.Count : array.Count;

            for (int i = 0; i < length; i++)
            {
                sum.x += array[flag ? indices[i] : i].x;
                sum.y += array[flag ? indices[i] : i].y;
                sum.z += array[flag ? indices[i] : i].z;
            }

            return sum / length;
        }

        /// <summary>
        /// Min for a set of 3d array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Vector2 Min(IList<Vector2> array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            int length = array.Count;
            float xMin = 0f, yMin = 0f;
            // first
            xMin = float.MaxValue;
            yMin = float.MaxValue;
            // 
            for (int i = 0; i < length; i++)
            {
                if (array[i].x < xMin)
                {
                    xMin = array[i].x;
                }

                if (array[i].y < yMin)
                {
                    yMin = array[i].y;
                }
            }

            return new Vector2(xMin, yMin);
        }

        /// <summary>
        /// Max for a set of 3d array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Vector2 Max(IList<Vector2> array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            int length = array.Count;
            // first
            float xMax = float.MinValue;
            float yMax = float.MinValue;
            // 
            for (int i = 0; i < length; i++)
            {
                if (array[i].x > xMax)
                {
                    xMax = array[i].x;
                }

                if (array[i].y > yMax)
                {
                    yMax = array[i].y;
                }
            }

            return new Vector2(xMax, yMax);
        }

        /// <summary>
        /// Rect for a set of 3d array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Rect GetRect(IList<Vector2> array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            Vector2 min = Min(array);
            Vector2 max = Max(array);

            return new Rect(min, max - min);
        }

        /// <summary>
        /// Normalize for a set of 2d array
        /// </summary>
        /// <param name="array"></param>
        public static void Normalize(IList<Vector2> array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            Rect rect = GetRect(array);
            float magnitude = rect.width > rect.height ? rect.width : rect.height;

            for (int i = 0; i < array.Count; i++)
            {
                Vector2 vector = array[i] - rect.min;
                array[i] = vector / magnitude;
            }
        }

        /// <summary>
        /// 通过一系列点，计算uvs
        /// </summary>
        /// <param name="points"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static Vector2[] CalculateUVs(IList<Vector3> points, Vector3 normal)
        {
            Vector2[] uvs = Projection.PlanerProject(points, null, normal);
            Normalize(uvs);
            return uvs;
        }

        /// <summary>
        /// Calculate the unit vector normal of 3 points.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Vector3 Normal(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            Vector3 a = p1 - p0;
            Vector3 b = p2 - p0;

            Vector3 cross = Vector3.zero;
            cross = Vector3.Cross(a, b);

            if (cross.magnitude < Mathf.Epsilon)
            {
                return new Vector3(0f, 0f, 0f); // bad triangle.
            }
            else
            {
                cross.Normalize();
                return cross;
            }
        }

        /// <summary>
        /// Calculate the normal for a set of 3d points.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public static Vector3 Normal(IList<Vector3> points, IList<int> indices = null)
        {
            bool flag = (indices != null && indices.Count > 3 && indices.Count % 3 == 0) ? true : false;
            Vector3 normal = Vector3.zero;
            if (flag)
            {
                normal = Normal(points[indices[0]], points[indices[1]], points[indices[2]]);
            }
            else
            {
                int length = points.Count;
                for (int p = length - 1, q = 0; q < length; p = q++)
                {
                    normal += Vector3.Cross(points[p] - points[0], points[q] - points[0]);
                }

                normal.Normalize();
            }

            return normal;
        }

        public static bool Overlap(this Vector2 p0, Vector2 p1)
        {
            if (Appr(p0, p1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 三点共线 （海伦公式）
        /// 海伦公式 S=sqrt(p(p-a)(p-b)(p-c)) p=(a+b+c)/2
        /// </summary>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static bool Collinear(Vector3 point0, Vector3 point1, Vector3 point2)
        {
            float a = Vector3.Distance(point0, point1);
            float b = Vector3.Distance(point1, point2);
            float c = Vector3.Distance(point0, point2);

            float p = (a + b + c) * 0.5f;
            float ss = p * (p - a) * (p - b) * (p - c);
            
            if (ss < 1e-8)
                return true;

            return false;
        }

        /// <summary>
        /// 去重
        /// </summary>
        /// <param name="source"></param>
        public static void Distinct(IList<Vector2> source)
        {
            int length = source.Count;
            for (int i = length - 1; i >= 0; i--)
            {
                Vector2 origin = source[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    if (Appr(origin, source[j]))
                    {
                        source.RemoveAt(j);
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// normal Atan2 returns in range [-pi,pi], this shifts to [0,2pi]
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>[0,2pi]</returns>
        public static float PositiveAtan2(Vector2 vector)
        {
            float angle = Mathf.Atan2(vector.y, vector.x);
            if (angle < 0)
                angle += 2 * Mathf.PI;

            return angle;
        }

        /// <summary>
        /// 
        /// [-2_PI,2_PI] and min < max
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float ClampAngleRad(float theta, float min, float max)
        {
            float c = (min + max) * 0.5f;
            float e = max - c;

            theta = theta % TwoPI;

            theta -= c;
            if (theta < -Mathf.PI)
            {
                theta += TwoPI;
            }
            else if (theta > Mathf.PI)
            {
                theta -= TwoPI;
            }

            if (theta < -e)
            {
                theta = -e;
            }
            else if (theta > e)
            {
                theta = e;
            }

            return theta + c;
        }


    }
}
