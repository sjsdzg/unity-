using UnityEngine;
using System.Collections;

namespace XFramework.Common
{
    public enum SceneSkipType
    {
        /// <summary>
        /// 按钮点击后，进入场景
        /// </summary>
        Button,
        /// <summary>
        /// 场景加载完成后，进入场景
        /// </summary>
        Complete,
        /// <summary>
        /// 按任意键后，进入场景
        /// </summary>
        AnyKey,
    }
}

