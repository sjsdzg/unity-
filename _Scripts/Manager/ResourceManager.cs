using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using XFramework.Core;

namespace XFramework
{
    /// <summary>
    /// 资源辅助信息
    /// </summary>
    public class AssetInfo
    {
        private UnityEngine.Object _Object;
        public Type AssetType { get; set; }
        public string Path { get; set; }
        public int RefCount { get; set; }
        public bool IsLoaded { get { return null != _Object; } }
        public UnityEngine.Object AssetObject
        {
            get
            {
                if (null == _Object)
                {
                    _ResourcesLoad();
                }
                return _Object;
            }
        }

        private void _ResourcesLoad()
        {
            try
            {
                _Object = Resources.Load(Path);
                if (null == _Object)
                    Debug.Log("Resources Load Failure! Path: " + Path);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
        }

        public IEnumerator GetCoroutineObject(Action<UnityEngine.Object> _loaded)
        {
            while (true)
            {
                yield return null;
                if (null == _Object)
                {
                    _ResourcesLoad();
                    yield return null;
                }
                if (null != _loaded)
                    _loaded(_Object);
                yield break;
            }
        }

        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> _loaded)
        {
            return GetAsyncObject(_loaded, null);
        }

        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            // have Object
            if (null != _Object)
            {
                _loaded(_Object);
                yield break;
            }
            // Object null, Not Load Resources
            ResourceRequest _resRequest = Resources.LoadAsync(Path);
            //
            while (_resRequest.progress < 0.9)
            {
                if (null != _progress)
                    _progress(_resRequest.progress);
                yield return null;
            }
            //
            while (!_resRequest.isDone)
            {
                if (null != _progress)
                    _progress(_resRequest.progress);
                yield return null;
            }
            //
            _Object = _resRequest.asset;
            if (null != _loaded)
                _loaded(_Object);
        }
    }
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>
	{
        private Dictionary<string, AssetInfo> dicAssetInfo = null;
        protected override void Init()
        {
            dicAssetInfo = new Dictionary<string, AssetInfo>();
        }

        #region Load Resources & Instantiate Object
        public UnityEngine.Object LoadInstance(string _path)
        {
            UnityEngine.Object _obj = Load(_path);
            return Instantiate(_obj);
        }

        public void LoadCoroutineInstance(string _path, Action<UnityEngine.Object> _instantiated)
        {
            LoadCoroutine(_path, (_obj) => { Instantiate(_obj, _instantiated); });
        }

        public void LoadAsyncInstance(string _path, Action<UnityEngine.Object> _instantiated)
        {
            LoadAsync(_path, (_obj) => { Instantiate(_obj, _instantiated); });
        }

        public void LoadAsyncInstance(string _path, Action<UnityEngine.Object> _instantiated, Action<float> _progress)
        {
            LoadAsync(_path, (_obj) => { Instantiate(_obj, _instantiated); }, _progress);
        }
        #endregion

        #region Load Resources
        /// <summary>
        /// Load the specified _path
        /// </summary>
        /// <param name="_path">_path</param>
        /// <returns></returns>
        public UnityEngine.Object Load(string _path)
        {
            AssetInfo _assetInfo = GetAssetInfo(_path);
            if (null != _assetInfo)
                return _assetInfo.AssetObject;
            return null;
        }
        #endregion

        #region Load Coroutine Resources
        public void LoadCoroutine(string _path, Action<UnityEngine.Object> _loaded)
        {
            AssetInfo _assetInfo = GetAssetInfo(_path, _loaded);
            if (null != _assetInfo)
                CoroutineManager.Instance.StartCoroutine(_assetInfo.GetCoroutineObject(_loaded));
        }
        #endregion

        #region Load Async Resources
        public void LoadAsync(string _path, Action<UnityEngine.Object> _loaded)
        {
            LoadAsync(_path, _loaded, null);
        }

        public void LoadAsync(string _path, Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            AssetInfo _assetInfo = GetAssetInfo(_path, _loaded);
            if (null != _assetInfo)
                CoroutineManager.Instance.StartCoroutine(_assetInfo.GetAsyncObject(_loaded, _progress));
        }
        #endregion

        #region gets the asset info
        /// <summary>
        /// gets the asset info
        /// </summary>
        /// <param name="_path">_path</param>
        /// <returns></returns>
        private AssetInfo GetAssetInfo(string _path)
        {
            return GetAssetInfo(_path, null);
        }
        /// <summary>
        /// gets the asset info
        /// </summary>
        /// <param name="_path">_path</param>
        /// <param name="_loaded">_loaded</param>
        /// <returns></returns>
        private AssetInfo GetAssetInfo(string _path, Action<UnityEngine.Object> _loaded)
        {
            if (string.IsNullOrEmpty(_path))
            {
                Debug.LogError("Error: null _path name");
                if (null != _loaded)
                    _loaded(null);
            }
            //Load Res....
            AssetInfo _asssetInfo = null;
            if (!dicAssetInfo.TryGetValue(_path,out _asssetInfo))
            {
                _asssetInfo = new AssetInfo();
                _asssetInfo.Path = _path;
                dicAssetInfo.Add(_path, _asssetInfo);
            }
            _asssetInfo.RefCount++;
            return _asssetInfo;
        }
        #endregion

        #region Instantiate Object
        private UnityEngine.Object Instantiate(UnityEngine.Object _obj)
        {
            return Instantiate(_obj, null);
        }

        private UnityEngine.Object Instantiate(UnityEngine.Object _obj, Action<UnityEngine.Object> _instantiated)
        {
            UnityEngine.Object _retObj = null;
            if (null != _obj)
            {
                _retObj = MonoBehaviour.Instantiate(_obj);
                if (null != _retObj)
                {
                    if (null != _instantiated)
                    {
                        _instantiated(_retObj);
                        return null;
                    }
                    return _retObj;
                }
                else
                {
                    Debug.LogError("Error: null Instantiate _retObj.");
                }
            }
            else
            {
                Debug.LogError("Error: null Resources Load return _obj");
            }
            return null;
        }
        #endregion
    }
}
