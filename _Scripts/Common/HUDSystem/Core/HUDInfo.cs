using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public abstract class HUDInfo
    {
        /// <summary>
        /// 目标
        /// </summary>
        public Transform m_Target = null;

        /// <summary>
        /// HUD类型
        /// </summary>
        public HUDType HUDType = HUDType.None;

        /// <summary>
        /// 相对目标的offset
        /// </summary>
        public Vector3 offset = Vector3.zero;

        /// <summary>
        /// 距离
        /// </summary>
        public float distance = 0f;

        /// <summary>
        /// 指示器Prefab
        /// </summary>
        public GameObject hudPrefab;
    }

    /// <summary>
    /// HUD信息
    /// </summary>
    [Serializable]
    public class HUDInfo<TOnScreenArgs, TOffScreenArags> : HUDInfo where TOnScreenArgs : HUDArgs where TOffScreenArags : HUDArgs
    {
        /// <summary>
        /// 屏幕内参数
        /// </summary>
        public TOnScreenArgs onScreenArgs;

        /// <summary>
        /// 屏幕外
        /// </summary>
        public TOffScreenArags offScreenArgs;
    }
}
