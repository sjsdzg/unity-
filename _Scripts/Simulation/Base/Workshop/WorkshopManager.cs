using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework
{
    /// <summary>
    /// 车间管理
    /// </summary>
    public class WorkshopManager : Singleton<WorkshopManager>
    {
        /// <summary>
        /// 车间信息数据类
        /// </summary>
        public class WorkshopInfoData
        {
            /// <summary>
            /// UI类型
            /// </summary>
            public EnumWorkshopType WorkshopType { get; private set; }

            public Type ScriptType { get; private set; }
            /// <summary>
            /// UI prefab 路径
            /// </summary>
            public string Path { get; private set; }
            /// <summary>
            /// UI参数
            /// </summary>
            public object[] WorkshopParams { get; private set; }

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="type"></param>
            /// <param name="path"></param>
            /// <param name="_params"></param>
            public WorkshopInfoData(EnumWorkshopType type, string path, params object[] _params)
            {
                this.WorkshopType = type;
                this.ScriptType = WorkshopDefine.GetWorkshopScript(type);
                this.Path = path;
                this.WorkshopParams = _params;
            }
        }

        /// <summary>
        /// 打开车间的字典
        /// </summary>
        private Dictionary<EnumWorkshopType, UnityEngine.GameObject> DictLoadWorkshops = null;

        /// <summary>
        /// 准备打开车间的栈
        /// </summary>
        private Stack<WorkshopInfoData> StackLoadWorkshops = null;

        /// <summary>
        /// Init this Singleton.
        /// </summary>
        protected override void Init()
        {
            DictLoadWorkshops = new Dictionary<EnumWorkshopType, UnityEngine.GameObject>();
            StackLoadWorkshops = new Stack<WorkshopInfoData>();
        }

        /// <summary>
        /// 车间是否加载
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsLoaded(EnumWorkshopType type)
        {
            if (DictLoadWorkshops.ContainsKey(type))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取车间对象
        /// </summary>
        /// <param name="type">界面类型</param>
        /// <returns></returns>
        public GameObject GetWorkshopObject(EnumWorkshopType type)
        {
            UnityEngine.GameObject retObj = null;
            if (!DictLoadWorkshops.TryGetValue(type, out retObj))
                throw new ApplicationException("DictLoadWorkshops TryGetValue Failure! EnumWorkshopType : " + type.ToString());
            return retObj;
        }

        /// <summary>
        /// 获取车间对象脚本组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public T GetWorkshopScript<T>(EnumWorkshopType type) where T : BaseWorkshop
        {
            UnityEngine.GameObject retObj = GetWorkshopObject(type);
            if (retObj != null)
            {
                return retObj.GetComponent<T>();
            }
            return null;
        }

        #region 加载车间
        /// <summary>
        /// 加载车间
        /// </summary>
        /// <param name="_workshopTypes">车间类型数组</param>
        public void LoadWorkshop(EnumWorkshopType[] _workshopTypes)
        {
            LoadWorkshop(false, _workshopTypes, null);
        }

        /// <summary>
        /// 加载车间
        /// </summary>
        /// <param name="_workshopType">车间类型</param>
        /// <param name="objParams">车间参数</param>
        public void LoadWorkshop(EnumWorkshopType _workshopType, params object[] objParams)
        {
            EnumWorkshopType[] _workshopTypes = new EnumWorkshopType[1];
            _workshopTypes[0] = _workshopType;
            LoadWorkshop(false, _workshopTypes, objParams);
        }

        /// <summary>
        /// 加载车间
        /// </summary>
        /// <param name="_workshopTypes">车间类型数组</param>
        public void LoadWorkshopCloseOthers(EnumWorkshopType[] _workshopTypes)
        {
            LoadWorkshop(true, _workshopTypes, null);
        }

        /// <summary>
        /// 加载车间
        /// </summary>
        /// <param name="_workshopType">车间类型</param>
        /// <param name="objparams">参数</param>
        public void LoadWorkshopCloseOthers(EnumWorkshopType _workshopType, params object[] objparams)
        {
            EnumWorkshopType[] _workshopTypes = new EnumWorkshopType[1];
            _workshopTypes[0] = _workshopType;
            LoadWorkshop(true, _workshopTypes, objparams);
        }

        /// <summary>
        /// 加载车间
        /// </summary>
        /// <param name="isCloseOthers">是否关闭其他车间</param>
        /// <param name="workshopTypes">车间类型数组</param>
        /// <param name="workshopParams">车间参数</param>
        private void LoadWorkshop(bool isCloseOthers,  EnumWorkshopType[] workshopTypes, params object[] workshopParams)
        {
            if (isCloseOthers)
            {
                UnloadWorkshopAll();
            }

            for (int i = 0; i < workshopTypes.Length; i++)
            {
                EnumWorkshopType _workshopType = workshopTypes[i];
                if (!DictLoadWorkshops.ContainsKey(_workshopType))
                {
                    string _path = WorkshopDefine.GetWorkshopPrefabPath(_workshopType);
                    WorkshopInfoData workshopInfoData = new WorkshopInfoData(_workshopType, _path, workshopParams);
                    StackLoadWorkshops.Push(workshopInfoData);
                }
            }

            if (StackLoadWorkshops.Count > 0)
            {
                CoroutineManager.Instance.StartCoroutine(AsyncLoadData());
            }
        }

        private IEnumerator AsyncLoadData()
        {
            WorkshopInfoData infoData = null;
            UnityEngine.Object _prefabObj = null;
            UnityEngine.GameObject _workshopObject = null;

            if (StackLoadWorkshops != null && StackLoadWorkshops.Count > 0)
            {
                do
                {
                    infoData = StackLoadWorkshops.Pop();
                    AsyncLoadAssetOperation async = Assets.LoadAssetAsync<GameObject>(infoData.Path);
                    yield return async;
                    _prefabObj = async.GetAsset<GameObject>();

                    if (_prefabObj != null)
                    {
                        _workshopObject = MonoBehaviour.Instantiate(_prefabObj) as UnityEngine.GameObject;
                        _workshopObject.name = WorkshopDefine.GetWorkshopName(infoData.WorkshopType);
                        BaseWorkshop _baseWorkshop = _workshopObject.GetComponent<BaseWorkshop>();
                        if (_baseWorkshop == null)
                        {
                            _baseWorkshop = _workshopObject.AddComponent(infoData.ScriptType) as BaseWorkshop;
                        }

                        if (_baseWorkshop != null)
                        {
                            _baseWorkshop.SetWorkshopWhenLoading(infoData.WorkshopParams);
                        }

                        DictLoadWorkshops.Add(infoData.WorkshopType, _workshopObject);
                    }
                } while (StackLoadWorkshops.Count > 0);
            }

            //yield return 0;
            yield return new WaitForEndOfFrame();
        }
        #endregion

        #region 卸载车间
        /// <summary>
        /// 卸载车间
        /// </summary>
        /// <param name="workshopType">车间类型</param>
        public void UnloadWorkshop(EnumWorkshopType workshopType)
        {
            UnityEngine.GameObject _obj = null;
            if (!DictLoadWorkshops.TryGetValue(workshopType, out _obj))
            {
                Debug.Log("DictLoadWorkshops TryGetValue Failure! EnumWorkshopType :" + workshopType.ToString());
                return;
            }
            UnloadWorkshop(workshopType, _obj);
        }

        /// <summary>
        /// 卸载车间
        /// </summary>
        /// <param name="workshopTypes">车间类型数组</param>
        public void UnloadWorkshop(EnumWorkshopType[] workshopTypes)
        {
            for (int i = 0; i < workshopTypes.Length; i++)
            {
                UnloadWorkshop(workshopTypes[i]);
            }
        }

        /// <summary>
        /// 卸载所有车间
        /// </summary>
        public void UnloadWorkshopAll()
        {
            List<EnumWorkshopType> mlist = new List<EnumWorkshopType>(DictLoadWorkshops.Keys);
            foreach (EnumWorkshopType workshopType in mlist)
            {
                UnityEngine.GameObject _obj = DictLoadWorkshops[workshopType];
                UnloadWorkshop(workshopType, _obj);
            }
            DictLoadWorkshops.Clear();
        }

        /// <summary>
        /// 卸载车间
        /// </summary>
        /// <param name="workshopType"></param>
        /// <param name="workshopObj"></param>
        private void UnloadWorkshop(EnumWorkshopType workshopType, UnityEngine.GameObject workshopObj)
        {
            if (workshopObj == null)
            {
                DictLoadWorkshops.Remove(workshopType);
            }
            else
            {
                BaseWorkshop _base = workshopObj.GetComponent<BaseWorkshop>();
                if (_base != null)
                {
                    _base.StateChanged += UnloadWorkshopHandler;
                    _base.Release();
                }
                else
                {
                    DictLoadWorkshops.Remove(workshopType);
                }
            }
        }

        /// <summary>
        /// 卸载车间后的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newState"></param>
        /// <param name="oldState"></param>
        private void UnloadWorkshopHandler(object sender, EnumObjectState newState, EnumObjectState oldState)
        {
            if (newState == EnumObjectState.Closing)
            {
                BaseWorkshop _base = sender as BaseWorkshop;
                DictLoadWorkshops.Remove(_base.GetWorkshopType());
                _base.StateChanged -= UnloadWorkshopHandler;
            }
        }
        #endregion
    }
}
