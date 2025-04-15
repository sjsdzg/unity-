using UnityEngine;
using System.Collections;
using XFramework.Core;
using XFramework.Common;
using System.Collections.Generic;
using System;

namespace XFramework.Core
{
    public class BaseScene : MonoBehaviour
    {

        #region Scene State
        private EnumObjectState state = EnumObjectState.Initial;
        /// <summary>
        /// 模块加载状态
        /// </summary>
        public EnumObjectState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    EnumObjectState oldState = state;
                    state = value;
                    if (null != StateChanged)
                    {
                        StateChanged(this, state, oldState);
                    }
                    OnStateChanged(state, oldState);
                }
            }
        }
        #endregion

        #region 模块状态改变事件
        public event StateChangedEventHandler StateChanged;
        #endregion

        protected virtual void OnStateChanged(EnumObjectState newState, EnumObjectState oldState)
        {
            
        }

        /// <summary>
        /// 预加载路径
        /// </summary>
        public List<string> PreloadPaths { get { return GetPreloadPaths(); } }

        private void Awake()
        {
            ScreenFader.Instance.FadeIn(0).Execute();
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            StartCoroutine(Initialize());
        }

        protected IEnumerator Initialize()
        {
            //初始化Asset
            if (!Assets.initialized)
            {

#if UNITY_EDITOR
                if (Assets.SimulateAssetBundleInEditor)
                {
                    yield return Assets.Initialize();
                }
                else
                {
                    yield return Assets.Initialize();
                }
#else
                yield return Assets.Initialize();
#endif
            }
            //解析场景参数
            ParseSceneParam();
            //预加载资源
            yield return Preload(PreloadPaths);
            Load();
        }



        /// <summary>
        /// 解析场景参数
        /// </summary>
        protected virtual void ParseSceneParam()
        {

        }

        /// <summary>
        /// 获取预加载资源
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> GetPreloadPaths()
        {
            return null;
        }

        /// <summary>
        /// 模块加载
        /// </summary>
        public void Load()
        {
            if (State != EnumObjectState.Initial)
                return;
            State = EnumObjectState.Loading;

            OnLoad();

            State = EnumObjectState.Ready;
        }

        protected virtual void OnLoad()
        {

        }

        /// <summary>
        /// 模块释放
        /// </summary>
        public void Release()
        {
            if (State != EnumObjectState.Disabled)
            {
                State = EnumObjectState.Closing;
                OnRelease();
                State = EnumObjectState.Disabled;
            }
        }

        protected virtual void OnRelease()
        {
            ScreenFader.Instance.FadeIn(0).Execute();
            Assets.UnloadAll();
        }

        /// <summary>
        /// 下载模型资源
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected IEnumerator Preload(List<string> paths)
        {
            if (paths == null)
                yield break;

            AssetBundlePreloadOperation async = Assets.LoadAssetBundleAsync(paths.ToArray());
            if (async != null)
            {
                async.OnUpdate(x =>
                {
                    AssetBundlePreloadOperation loader = x as AssetBundlePreloadOperation;
                    if (loader.Async != null)
                    {
                        LoadingBar.Instance.Show(loader.Progress, "正在下载：[" + loader.Async.assetBundleName + "]...");
                    }
                });
            }

            yield return async;

            LoadingBar.Instance.Show(1, "资源下载完成。");
            yield return new WaitForSeconds(0.2f);
            LoadingBar.Instance.Hide();
        }
    }
}

