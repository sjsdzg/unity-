using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    public class SplinePoint : IEquatable<SplinePoint>
    {
        public enum TangentMode { SmoothMirrored, Broken, SmoothFree}

        private TangentMode _mode;
        /// <summary>
        /// 切线模式
        /// </summary>
        public TangentMode mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public Vector3 position;
        public Color color;
        public Vector3 normal;
        public float size;
        public Vector3 tangent;
        public Vector3 tangent2;

        public void SetPosition(Vector3 pos)
        {
            tangent += pos - position;
            tangent2 += pos - position;
            position = pos;
        }

        public void SetTangent(Vector3 pos)
        {
            tangent = pos;
            switch (_mode)
            {
                case TangentMode.SmoothMirrored:
                    SmoothMirroredTangent2();
                    break;
                case TangentMode.SmoothFree:
                    SmoothFreeTangent2();
                    break;
                default:
                    break;
            }
        }

        public void SetTangent2(Vector3 pos)
        {
            tangent2 = pos;
            switch (_mode)
            {
                case TangentMode.SmoothMirrored:
                    SmoothMirroredTangent();
                    break;
                case TangentMode.SmoothFree:
                    SmoothFreeTangent();
                    break;
                default:
                    break;
            }
        }

        private void SmoothMirroredTangent2()
        {
            tangent2 = position + (position - tangent);
        }

        private void SmoothFreeTangent2()
        {
            tangent2 = position + (position - tangent).normalized * (tangent2 - position).magnitude;
        }

        private void SmoothMirroredTangent()
        {
            tangent = position + (position - tangent2);
        }

        private void SmoothFreeTangent()
        {
            tangent = position + (position - tangent2).normalized * (tangent - position).magnitude;
        }

        /// <summary>
        /// 是否相同
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SplinePoint other)
        {
            if (this.mode.Equals(other.mode) &&
                this.position.Equals(other.position) &&
                this.color.Equals(other.color) &&
                this.normal.Equals(other.normal) &&
                this.size.Equals(other.size) &&
                this.tangent.Equals(other.tangent) &&
                this.tangent.Equals(other.tangent2))
            {
                return true;
            }

            return false;
        }
    }
}
