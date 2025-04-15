using UnityEngine;
using System.Collections;
using XFramework.Core;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace XFramework.Common
{
    /// <summary>
    /// 场景加载器
    /// </summary>
    public class SceneLoader : Singleton<SceneLoader>
    {
        /// <summary>
        /// 场景加载信息设置
        /// </summary>
        public SceneLoaderSettings Settings = null;

        /// <summary>
        /// 场景参数字典
        /// </summary>
        private Dictionary<SceneType, SceneParam> m_SceneParams;

        /// <summary>
        /// 异步加载场景的名称
        /// </summary>
        public SceneType AsyncSceneType { get; private set; }

        protected override void Init()
        {
            base.Init();
            Settings = SceneLoaderUtils.GetSceneLoaderSettings();
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            m_SceneParams = new Dictionary<SceneType, SceneParam>();
        }

        /// <summary>
        /// 场景加载完成时，触发
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.LogFormat("[Scene] : {0} is loaded, LoadSceneMode : {1}", scene.name, mode);
        }

        /// <summary>
        /// 获取对应场景的传递参数
        /// </summary>
        /// <param name="sceneType"></param>
        /// <returns></returns>
        public SceneParam GetSceneParam(SceneType sceneType)
        {
            SceneParam param = null;
            m_SceneParams.TryGetValue(sceneType, out param);
            return param;
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="_sceneType"></param>
        public void LoadSceneAsync(SceneType sceneType, SceneParam param = null)
        {
            sceneType = RemapSceneType(sceneType);

            if (param == null)
                param = new SceneParam();

            if (m_SceneParams.ContainsKey(sceneType))
                m_SceneParams[sceneType] = param;
            else
                m_SceneParams.Add(sceneType, param);
            //释放场景
            BaseScene baseScene = GameObject.FindObjectOfType<BaseScene>();
            if (baseScene != null)
                baseScene.Release();
            //先加载过渡场景，然后加载当前场景
            AsyncSceneType = sceneType;
            SceneLoaderInfo loadingSceneInfo = Settings.GetInfo(SceneType.LoadingScene);
            //Assets.LoadSceneAsync(loadingSceneInfo.AssetBundleName, loadingSceneInfo.SceneName, false);
            SceneManager.LoadScene(SceneType.LoadingScene.ToString());
        }

        private SceneType RemapSceneType(SceneType sceneType)
        {
            if (sceneType == SceneType.ProductionMainScene)
            {
#if UNITY_WEBGL
                sceneType = SceneType.ProductionMainScene;
#else
                sceneType = SceneType.ProductionMainScene;
#endif

            }
            return sceneType;
        }
    }

    /// <summary>
    /// 场景传递的参数基类
    /// </summary>
    public class SceneParam
    {
        /// <summary>
        /// 来自哪个Scene
        /// </summary>
        public string FromScene { get; private set; }

        /// <summary>
        /// 显式默认构造函数
        /// </summary>
        public SceneParam()
        {
            FromScene = SceneManager.GetActiveScene().name;
        }
    }
}

                                                  