using UnityEngine;
using System.Collections;
using XFramework.Core;

namespace XFramework.Common
{
    public static class SceneLoaderUtils
    {
        /// <summary>
        /// 获取场景加载信息设置
        /// </summary>
        /// <returns></returns>
        public static SceneLoaderSettings GetSceneLoaderSettings()
        {
            return Resources.Load<SceneLoaderSettings>("SceneLoaderSettings");
        }

        //public static AsyncLoadOperation GetSceneLoaderSettings()
        //{
        //    AsyncLoadOperation operation = Assets.LoadAssetAsync<SceneLoaderSettings>("Settings/SceneLoaderSettings", "SceneLoaderSettings");
        //    operation.OnCompleted(async =>
        //    {
        //        if (string.IsNullOrEmpty(async.Error))
        //        {
        //            AssetBundleLoadAssetOperation loadAssetAsync = operation as AssetBundleLoadAssetOperation;
        //            Settings = loadAssetAsync.GetAsset<SceneLoaderSettings>();
        //        }
        //        else
        //        {
        //            Debug.LogError(async.Error);
        //        }
        //    });
        //    return operation;
        //}

        public static AssetBundlePreloadOperation LoadAsssetBundleAsync(string sceneName)
        {
            SceneLoaderInfo info = SceneLoader.Instance.Settings.GetInfo(sceneName);
            AssetBundlePreloadOperation operation = Assets.LoadAssetBundleAsync(info.AssetBundleName);
            return operation;
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="isAdditive"></param>
        /// <returns></returns>
        public static AsyncLoadSceneOperation LoadSceneAsync(string sceneName, bool isAdditive)
        {
            SceneLoaderInfo info = SceneLoader.Instance.Settings.GetInfo(sceneName);
            AsyncLoadOperation operation = Assets.LoadSceneAsync(info.AssetBundleName, info.SceneName, isAdditive);
            return operation as AsyncLoadSceneOperation;
        }
    }
}

