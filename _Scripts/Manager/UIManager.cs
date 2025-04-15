using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;

namespace XFramework
{
    class UIManager : Singleton<UIManager>
    {
        /// <summary>
        /// UI信息数据类
        /// </summary>
        class UIInfoData
        {
            /// <summary>
            /// UI类型
            /// </summary>
            public EnumUIType UIType { get; private set; }

            public Type ScriptType { get; private set; }
            /// <summary>
            /// UI prefab 路径
            /// </summary>
            public string Path { get; private set; }
            /// <summary>
            /// UI参数
            /// </summary>
            public object[] UIParams { get; private set; }

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="type"></param>
            /// <param name="path"></param>
            /// <param name="_params"></param>
            public UIInfoData(EnumUIType type,string path,params object[] _params)
            {
                this.UIType = type;
                this.ScriptType = UIDefine.GetUIScript(type);
                this.Path = path;
                this.UIParams = _params;
            }
        }

        /// <summary>
        /// 打开界面的字典
        /// </summary>
        private Dictionary<EnumUIType, GameObject> DictOpenUIs = null;

        /// <summary>
        /// 准备打开界面的栈
        /// </summary>
        private Stack<UIInfoData> StackOpenUIs = null;

        /// <summary>
        /// Init this Singleton.
        /// </summary>
        protected override void Init()
        {
            DictOpenUIs = new Dictionary<EnumUIType, GameObject>();
            StackOpenUIs = new Stack<UIInfoData>();
        }

        /// <summary>
        /// 获取界面对象
        /// </summary>
        /// <param name="type">界面类型</param>
        /// <returns></returns>
        public GameObject GetUIObject(EnumUIType type)
        {
            GameObject retObj = null;
            if (!DictOpenUIs.TryGetValue(type, out retObj))
                throw new ApplicationException("DictOpenUIs TryGetValue Failure!_uiType: " + type.ToString());
            return retObj;
        }

        /// <summary>
        /// 获取界面对象脚本组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public T GetUI<T>(EnumUIType type) where T : BaseUI
        {
            GameObject retObj = GetUIObject(type);
            if (retObj != null)
            {
                return retObj.GetComponent<T>();
            }
            return null;
        }


        #region 打开界面
        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiTypes">界面类型数组</param>
        public void OpenUI(EnumUIType[] uiTypes)
        {
            OpenUI(false, uiTypes, null);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiType">界面类型</param>
        /// <param name="uiObjParams">界面参数</param>
        public void OpenUI(EnumUIType uiType, params object[] uiObjParams)
        {
            EnumUIType[] uiTypes = new EnumUIType[1];
            uiTypes[0] = uiType;
            OpenUI(false, uiTypes, uiObjParams);
        }

        /// <summary>
        /// 打开界面，关闭其他界面
        /// </summary>
        /// <param name="uiTypes">界面类型数组</param>
        public void OpenUICloseOthers(EnumUIType[] uiTypes)
        {
            OpenUI(true, uiTypes, null);
        }

        /// <summary>
        /// 打开界面，关闭其他界面
        /// </summary>
        /// <param name="uiType">界面类型</param>
        /// <param name="uiObjParams">界面参数</param>
        public void OpenUICloseOthers(EnumUIType uiType, params object[] uiObjParams)
        {
            EnumUIType[] uiTypes = new EnumUIType[1];
            uiTypes[0] = uiType;
            OpenUI(true, uiTypes, uiObjParams);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="isCloseOthers">是否关闭其他UI</param>
        /// <param name="uiTypes">UI类型数组</param>
        /// <param name="uiParams">参数</param>
        private void OpenUI(bool isCloseOthers, EnumUIType[] uiTypes, params object[] uiParams)
        {
            // Close Others UI.
            if (isCloseOthers)
            {
                CloseUIAll();
            }

            // push _uiTypes in Stack.
            for (int i = 0; i < uiTypes.Length; i++)
            {
                EnumUIType _uiType = uiTypes[i];
                if (!DictOpenUIs.ContainsKey(_uiType))
                {
                    string _path = UIDefine.GetUIPrefabPath(_uiType);
                    UIInfoData uiInfoData = new UIInfoData(_uiType, _path, uiParams);
                    StackOpenUIs.Push(uiInfoData);
                }
            }

            // Open UI.
            if (StackOpenUIs.Count > 0)
            {
                CoroutineManager.Instance.StartCoroutine(AsyncLoadData());
            }
        }

        private IEnumerator AsyncLoadData()
        {
            UIInfoData _uiInfoData = null;
            UnityEngine.Object _prefabObj = null;
            GameObject _uiObject = null;

            if (StackOpenUIs != null && StackOpenUIs.Count > 0)
            {
                do
                {
                    _uiInfoData = StackOpenUIs.Pop();
                    AsyncLoadAssetOperation async = Assets.LoadAssetAsync<GameObject>(_uiInfoData.Path);
                    yield return async;
                    if (async != null)
                        _prefabObj = async.GetAsset<GameObject>();

                    if (_prefabObj != null)
                    {
                        _uiObject = MonoBehaviour.Instantiate(_prefabObj) as GameObject;
                        BaseUI _baseUI = _uiObject.GetComponent<BaseUI>();
                        if (null == _baseUI)
                        {
                            _baseUI = _uiObject.AddComponent(_uiInfoData.ScriptType) as BaseUI;
                        }
                        if (null != _baseUI)
                        {
                            _baseUI.SetUIWhenOpening(_uiInfoData.UIParams);
                        }
                        DictOpenUIs.Add(_uiInfoData.UIType, _uiObject);
                    }
                } while (StackOpenUIs.Count > 0);
            }
            yield return 0;
        }
        #endregion

        #region 关闭界面
        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="uiType">界面类型</param>
        public void CloseUI(EnumUIType uiType)
        {
            GameObject uiObj = null;
            if (!DictOpenUIs.TryGetValue(uiType, out uiObj))
            {
                Debug.Log("dicOpenUIs TryGetValue Failure! _uiType :" + uiType.ToString());
                return;
            }
            CloseUI(uiType, uiObj);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="uiTypes">界面类型数组</param>
        public void CloseUI(EnumUIType[] uiTypes)
        {
            for (int i = 0; i < uiTypes.Length; i++)
            {
                CloseUI(uiTypes[i]);
            }
        }

        /// <summary>
        /// 关闭所有UI界面
        /// </summary>
        public void CloseUIAll()
        {
            List<EnumUIType> _keyList = new List<EnumUIType>(DictOpenUIs.Keys);
            foreach (EnumUIType _uiType in _keyList)
            {
                GameObject _uiObj = DictOpenUIs[_uiType];
                CloseUI(_uiType, _uiObj);
            }
            DictOpenUIs.Clear();
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="uiType">界面类型</param>
        /// <param name="uiObj">界面对象</param>
        private void CloseUI(EnumUIType uiType, GameObject uiObj)
        {
            if (uiObj == null)
            {
                DictOpenUIs.Remove(uiType);
            }
            else
            {
                BaseUI _baseUI = uiObj.GetComponent<BaseUI>();
                if (_baseUI != null)
                {
                    _baseUI.StateChanged += CloseUIHandler;
                    _baseUI.Release();
                }
                else
                {
                    GameObject.Destroy(uiObj);
                    DictOpenUIs.Remove(uiType);
                }
            }
        }

        /// <summary>
        /// 关闭UI后的操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newState"></param>
        /// <param name="oldState"></param>
        void CloseUIHandler(object sender, EnumObjectState newState, EnumObjectState oldState)
        {
            if (newState == EnumObjectState.Closing)
            {
                BaseUI baseUI = sender as BaseUI;
                DictOpenUIs.Remove(baseUI.GetUIType());
                baseUI.StateChanged -= CloseUIHandler;
            }
        }
        #endregion
    }
}
