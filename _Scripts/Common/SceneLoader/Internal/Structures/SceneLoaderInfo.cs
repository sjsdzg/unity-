using UnityEngine;
using System.Collections;
using System;

namespace XFramework.Common
{
    [Serializable]
    public class SceneLoaderInfo
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        [Header("Settings")]
        public string SceneName = "";
        /// <summary>
        /// AssetBundle名称
        /// </summary>
        public string AssetBundleName = "";
        /// <summary>
        /// 场景跳过类型
        /// </summary>
        public SceneSkipType SkipType = SceneSkipType.Complete;
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName = "";
        /// <summary>
        /// 描述
        /// </summary>
        [TextArea(3, 7)]
        public string Description = "";
        /// <summary>
        /// 提示信息
        /// </summary>
        public string[] Tips = null;
        /// <summary>
        /// 引用背景
        /// </summary>
        [Header("References")]
        public Sprite[] Backgrounds = null;
    }
}
