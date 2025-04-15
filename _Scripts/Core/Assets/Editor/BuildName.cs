using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Core
{
    /// <summary>
    /// 构建AssetBundle的Name
    /// </summary>
    [Serializable]
    public class BuildName
    {
        [Header("Settings")]
        /// <summary>
        /// 查找文件夹路径
        /// </summary>
        public string seatchDirectory = "Assets";

        /// <summary>
        /// 查找模式
        /// </summary>
        public string searchPattern = "*.prefab";

        /// <summary>
        /// 查找选项
        /// </summary>
        public SearchOption searchOption = SearchOption.AllDirectories;

        /// <summary>
        /// 构建规则选项
        /// </summary>
        public BuildNameOption buildRuleOption = BuildNameOption.DirectroyWithFileName;

        /// <summary>
        /// AssetBundle Name
        /// </summary>
        public string assetBundleName;

        /// <summary>
        /// AssetBundle Variant
        /// </summary>
        public string assetBundleVariant;
    }
}
