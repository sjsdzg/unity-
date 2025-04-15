using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    public enum BuildNameOption
    {
        /// <summary>
        /// 根据Assets路径和文件名设置AssetBundleName
        /// </summary>
        AssetsWithFileName,
        /// <summary>
        /// 根据Assets路径和文件夹名设置AssetBundleName
        /// </summary>
        AssetsWithDirectroyName,
        /// <summary>
        /// 根据AssetBundleName设置AssetBundleName
        /// </summary>
        AssetBundleName,
        /// <summary>
        /// 根据文件名设置AssetBundleName
        /// </summary>
        FileName,
        /// <summary>
        /// 根据文件夹名设置AssetBundleName
        /// </summary>
        DirectroyName,
        /// <summary>
        /// 根据文件夹名和文件名设置AssetBundleName
        /// </summary>
        DirectroyWithFileName,

    }
}
