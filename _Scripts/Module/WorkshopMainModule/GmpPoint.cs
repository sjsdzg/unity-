using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Module { 
  

    /// <summary>
    /// GMP内容
    /// </summary>
    public class GmpPoint:IntroduceContent
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// 角度
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// 重要物体名字
        /// </summary>
        public string  ImpObject { get; set; }

        /// <summary>
        /// 相机模式
        /// </summary>
        public CameraMode CameraMode { get; set; }
    }

}
