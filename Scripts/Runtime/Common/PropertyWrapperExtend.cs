using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Runtime
{
    public static class PropertyWrapperExtend
    {
        public static PropertyWrapper<float> GetHandleSizeWrapper(this Transform transform, float screenSize)
        {
            PropertyWrapper<float> wrapper = new PropertyWrapper<float>(() => HandleUtility.GetHandleSize(transform.position) * screenSize);
            return wrapper;
        }

        public static PropertyWrapper<Vector3> GetHandleOffsetWrapper(this Transform transform, Vector3 offset, float screenSize)
        {
            PropertyWrapper<Vector3> wrapper = new PropertyWrapper<Vector3>(() => HandleUtility.GetHandleSize(transform.position) * offset * screenSize);
            return wrapper;
        }

        public static PropertyWrapper<Vector3> GetPositionWrapper(this Transform transform)
        {
            PropertyWrapper<Vector3> wrapper = new PropertyWrapper<Vector3>(() => transform.position, value => transform.position = value);
            return wrapper;
        }
    }
}
