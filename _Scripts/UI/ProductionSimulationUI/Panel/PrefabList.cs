using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// prefab列表类
    /// </summary>
    public class PrefabList : MonoBehaviour
    {
        /// <summary>
        /// prefab列表
        /// </summary>
        public List<PrefabInfo> mlist = new List<PrefabInfo>();

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetPrefab(string name)
        {
            GameObject prefab = null;
            PrefabInfo info = mlist.FirstOrDefault(x => x.name == name);
            if (info != null)
            {
                prefab = info.prefab;
            }
            return prefab;
        }

        [ContextMenu("自动生成名称")]
        void AutoBuildName()
        {
            foreach (var imageInfo in mlist)
            {
                imageInfo.name = imageInfo.prefab.name;
            }
        }
    }

    [Serializable]
    public class PrefabInfo
    {
        public string name;
        public GameObject prefab;
    }
}
