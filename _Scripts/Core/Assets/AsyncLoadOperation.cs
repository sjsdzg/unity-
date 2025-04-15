using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using XFramework.Core;
using System;
using System.Collections.Generic;
using System.Collections;

namespace XFramework.Core
{
    #region AsyncLoadOperation
    public abstract class AsyncLoadOperation : IEnumerator
    {
        public object Current
        {
            get
            {
                return null;
            }
        }

        public bool MoveNext()
        {
            return !IsDone;
        }

        public void Reset()
        {
        }

        abstract public bool Update();

        /// <summary>
        /// 中止
        /// </summary>
        public virtual void Abort()
        {

        }

        /// <summary>
        /// 是否开始
        /// </summary>
        public bool HasStarted { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsDone { get; protected set; }

        /// <summary>
        /// 进度
        /// </summary>
        public float Progress { get; protected set; }

        /// <summary>
        /// error
        /// </summary>
        public string Error { get; protected set; }

        private UniEvent<AsyncLoadOperation> m_OnStartEvent = new UniEvent<AsyncLoadOperation>();
        /// <summary>
        /// 开始时触发
        /// </summary>
        public UniEvent<AsyncLoadOperation> OnStartEvent
        {
            get { return m_OnStartEvent; }
            set { m_OnStartEvent = value; }
        }

        private UniEvent<AsyncLoadOperation> m_OnUpdateEvent = new UniEvent<AsyncLoadOperation>();
        /// <summary>
        /// 更新时触发
        /// </summary>
        public UniEvent<AsyncLoadOperation> OnUpdateEvent
        {
            get { return m_OnUpdateEvent; }
            set { m_OnUpdateEvent = value; }
        }

        private UniEvent<AsyncLoadOperation> m_OnCompletedEvent = new UniEvent<AsyncLoadOperation>();
        /// <summary>
        /// 完成时触发
        /// </summary>
        public UniEvent<AsyncLoadOperation> OnCompletedEvent
        {
            get { return m_OnCompletedEvent; }
            set { m_OnCompletedEvent = value; }
        }

        /// <summary>
        /// AssetBundle加载过程中通知
        /// </summary>
        protected virtual void Notify()
        {
            try
            {
                if (!HasStarted)
                {
                    OnStartEvent.Invoke(this);
                    HasStarted = true;
                }

                OnUpdateEvent.Invoke(this);

                if (IsDone)
                {
                    OnCompletedEvent.Invoke(this);
                }
            }
            catch (Exception e)
            {
                IsDone = true;
                Error = e.Message;
                Debug.LogException(e);
            }
        }
    }
    #endregion

    #region AssetBundleLoadOperation
    public abstract class AssetBundleLoadOperation : AsyncLoadOperation
    {
        public string assetBundleName { get; private set; }
        public LoadedAssetBundle assetBundle { get; protected set; }

        public LoadOptions LoadOption { get; protected set; }

        protected abstract bool downloadIsDone { get; }
        protected abstract void FinishDownload();

        public override bool Update()
        {
            if (!IsDone && downloadIsDone)
            {
                FinishDownload();
                IsDone = true;
            }

            Notify();

            return !IsDone;
        }

        public abstract string GetSourceURL();

        public AssetBundleLoadOperation(string assetBundleName, LoadOptions loadedOption)
        {
            this.assetBundleName = assetBundleName;
            LoadOption = loadedOption;
        }
    }

    /// <summary>
    /// 从Web加载AssetBundle
    /// </summary>
    public class AssetBundleLoadFromWebOperation : AssetBundleLoadOperation
    {
        UnityWebRequest m_Request;
        AsyncOperation m_Operation;
        string m_Url;

        public AssetBundleLoadFromWebOperation(string assetBundleName, UnityWebRequest request, LoadOptions loadOption = LoadOptions.Lazyload)
            : base(assetBundleName, loadOption)
        {
            if (request == null || !(request.downloadHandler is DownloadHandlerAssetBundle))
                throw new System.ArgumentNullException("UnityWebRequest");
            m_Url = request.url;
            m_Request = request;
            m_Operation = request.Send();
        }

        public override bool Update()
        {
            if (m_Operation != null)
                Progress = m_Operation.progress;

            return base.Update();
        }

        protected override bool downloadIsDone { get { return (m_Operation == null) || m_Operation.isDone; } }

        protected override void FinishDownload()
        {
            Error = m_Request.error;
            if (!string.IsNullOrEmpty(Error))
                return;

            var handler = m_Request.downloadHandler as DownloadHandlerAssetBundle;
            AssetBundle bundle = handler.assetBundle;
            if (bundle == null)
                Error = string.Format("{0} is not a valid asset bundle.", assetBundleName);
            else
                assetBundle = new LoadedAssetBundle(bundle, LoadOption);

            m_Request.Dispose();
            m_Request = null;
            m_Operation = null;
        }

        public override string GetSourceURL()
        {
            return m_Url;
        }
    }
    #endregion

    #region AsyncLoadSceneOperation
    public abstract class AsyncLoadSceneOperation : AsyncLoadOperation
    {
        protected AsyncOperation m_Request;

        /// <summary>
        /// 是否允许场景激活
        /// </summary>
        public bool AllowSceneActivation
        {
            get { return m_Request.allowSceneActivation; }
            set { m_Request.allowSceneActivation = value; }
        }
    }


#if UNITY_EDITOR
    public class AssetBundleLoadSceneSimulationOperation : AsyncLoadSceneOperation
    {
        public AssetBundleLoadSceneSimulationOperation(string assetBundleName, string levelName, bool isAdditive)
        {
            string[] levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, levelName);
            if (levelPaths.Length == 0)
            {
                ///@TODO: The error needs to differentiate that an asset bundle name doesn't exist
                //        from that there right scene does not exist in the asset bundle...
                Error = "There is no scene with name \"" + levelName + "\" in " + assetBundleName;
                Debug.LogError(Error);
                return;
            }

            if (isAdditive)
                m_Request = UnityEditor.EditorApplication.LoadLevelAdditiveAsyncInPlayMode(levelPaths[0]);
            else
                m_Request = UnityEditor.EditorApplication.LoadLevelAsyncInPlayMode(levelPaths[0]);
        }

        public override bool Update()
        {
            if (AllowSceneActivation)
            {
                Progress = m_Request.progress;
                IsDone = m_Request.isDone;
            }
            else
            {
                Progress = m_Request.progress + 0.1f;
                IsDone = Progress == 1 ? true : false;
            }

            Notify();

            return !IsDone;
        }

    }
#endif

    public class AssetBundleLoadSceneOperation : AsyncLoadSceneOperation
    {
        protected string m_AssetBundleName;
        protected string m_LevelName;
        protected bool m_IsAdditive;
        protected string m_DownloadingError;


        public AssetBundleLoadSceneOperation(string assetbundleName, string levelName, bool isAdditive)
        {
            m_AssetBundleName = assetbundleName;
            m_LevelName = levelName;
            m_IsAdditive = isAdditive;
        }

        public override bool Update()
        {
            LoadedAssetBundle bundle = Assets.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);

            if (m_Request == null && m_DownloadingError != null)
            {
                Error = m_DownloadingError;
                Debug.LogError(m_DownloadingError);
                IsDone = true;
            }

            if (bundle != null && m_Request == null)
                m_Request = SceneManager.LoadSceneAsync(m_LevelName, m_IsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);

            if (m_Request != null)
            {
                if (AllowSceneActivation)
                {
                    Progress = m_Request.progress;
                    IsDone = m_Request.isDone;
                }
                else
                {
                    Progress = m_Request.progress + 0.1f;
                    IsDone = m_Request.progress >= 0.9f ? true : false;
                }
            }

            Notify();

            return !IsDone;
        }

    }
    #endregion

    #region AsyncLoadAssetOperation
    public abstract class AsyncLoadAssetOperation : AsyncLoadOperation
    {
        public abstract T GetAsset<T>() where T : UnityEngine.Object;

        public abstract object[] GetAllAssets();
    }

#if UNITY_EDITOR
    public class AssetBundleLoadAssetOperationSimulation : AsyncLoadAssetOperation
    {
        UnityEngine.Object m_SimulatedObject;

        public AssetBundleLoadAssetOperationSimulation(UnityEngine.Object simulatedObject)
        {
            m_SimulatedObject = simulatedObject;
        }

        public override T GetAsset<T>()
        {
            return m_SimulatedObject as T;
        }

        public override object[] GetAllAssets()
        {
            return null;
        }

        public override bool Update()
        {
            Progress = 1;
            IsDone = true;
            Notify();

            return !IsDone;
        }
    }
#endif


    public class AssetBundleLoadAssetOperation : AsyncLoadAssetOperation
    {
        protected string m_AssetBundleName;
        protected string m_AssetName;
        protected string m_DownloadingError;
        protected System.Type m_Type;
        protected AssetBundleRequest m_Request = null;

        public AssetBundleLoadAssetOperation(string bundleName, string assetName, System.Type type)
        {
            m_AssetBundleName = bundleName;
            m_AssetName = assetName;
            m_Type = type;
        }

        protected virtual void FinishDownload()
        {

        }

        public override T GetAsset<T>()
        {
            if (m_Request != null && m_Request.isDone)
                return m_Request.asset as T;
            else
                return null;
        }
        public override object[] GetAllAssets()
        {
            LoadedAssetBundle bundle = Assets.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
            if (bundle != null)
            {
                return bundle.LoadAllAssets();
            }
            else
                return null;
        }

        // Returns true if more Update calls are required.
        public override bool Update()
        {
            LoadedAssetBundle bundle = Assets.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
            if (m_Request == null && m_DownloadingError != null)
            {
                Debug.LogError(m_DownloadingError);
                IsDone = true;
            }

            if (bundle != null && m_Request == null)
                m_Request = bundle.m_AssetBundle.LoadAssetAsync(m_AssetName, m_Type);

            if (m_Request != null)
            {
                Progress = m_Request.progress;
                IsDone = m_Request.isDone;
                //完成加载
                if (m_Request.isDone)
                {
                    FinishDownload();
                }
            }

            Notify();

            return !IsDone;
        }
    }

    public class AssetBundleLoadManifestOperation : AssetBundleLoadAssetOperation
    {
        public AssetBundleLoadManifestOperation(string bundleName, string assetName, System.Type type)
            : base(bundleName, assetName, type)
        {

        }

        protected override void FinishDownload()
        {
            base.FinishDownload();
            Assets.AssetBundleManifestObject = GetAsset<AssetBundleManifest>();
        }
    }
    #endregion

    public class AssetBundlePreloadOperation : AsyncLoadOperation
    {
        /// <summary>
        /// 需要预加载的AssetBundle
        /// </summary>
        public List<string> AssetBundleNames { get; private set; }

        /// <summary>
        /// 加载器
        /// </summary>
        public AssetBundleLoadOperation Async { get; private set; }

        private int index = -1;
        /// <summary>
        /// 进度索引
        /// </summary>
        public int Index
        {
            get { return index; }
            private set
            {
                if (index == value)
                    return;

                index = value;
                Next();
            }
        }

        public AssetBundlePreloadOperation(List<string> assetBundleNames)
        {
            if (assetBundleNames == null)
                throw new System.ArgumentNullException("AssetBundlePreloadOperation");
            AssetBundleNames = assetBundleNames;
            Index = 0;
        }

        /// <summary>
        /// 判断下一个
        /// </summary>
        private void Next()
        {
            if (Index < AssetBundleNames.Count)
            {
                Async = Assets.LoadAssetBundleExternal(AssetBundleNames[Index]);
            }
        }

        public override bool Update()
        {
            if (Async != null)
                Progress = (Index + Async.Progress) / AssetBundleNames.Count;
            else
                Progress = (float)Index / AssetBundleNames.Count;

            if (Async == null || Async.IsDone)
                Index++;

            if (Index == AssetBundleNames.Count)
            {
                Progress = 1;
                IsDone = true;
            }

            Notify();

            return !IsDone;
        }
    }

}
