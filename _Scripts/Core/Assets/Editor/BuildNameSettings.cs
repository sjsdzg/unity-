using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SubjectNerd.Utilities;
using UnityEngine;

namespace XFramework.Core
{
    /// <summary>
    /// 构建AssetBundle的名称设置
    /// </summary>
    public class BuildNameSettings : ScriptableObject
    {
        [Header("Build Name Settings")]
        [Reorderable]
        public List<BuildName> BuildNames = new List<BuildName>();
    }
}
