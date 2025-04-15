using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SubjectNerd.Utilities;

namespace XFramework.Common
{
    public class SceneLoaderSettings : ScriptableObject
    {
        [Header("Scene Loader Settings")]
        [Reorderable]
        public List<SceneLoaderInfo> SceneLoaderInfos = new List<SceneLoaderInfo>();

        public SceneLoaderInfo GetInfo(string sceneName)
        {
            foreach (SceneLoaderInfo info in SceneLoaderInfos)
            {
                if (info.SceneName.Equals(sceneName))
                {
                    return info;
                }
            }

            Debug.Log("Not found any scene with this name: " + sceneName);
            return null;
        }

        public SceneLoaderInfo GetInfo(SceneType sceneType)
        {
            return GetInfo(sceneType.ToString());
        }
    }
}
