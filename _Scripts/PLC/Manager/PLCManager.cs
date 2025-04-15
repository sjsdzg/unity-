using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;
using XFramework.PLC;

namespace XFramework.PLC
{
    public class PLCManager : Singleton<PLCManager>
    {
        /// <summary>
        /// PLC信息数据类
        /// </summary>
        public class PLC_InfoData
        {
            /// <summary>
            /// PLC类型
            /// </summary>
            public PLC_Type PLC_Type { get; private set; }

            public Type ScriptType { get; private set; }
            /// <summary>
            /// PLC prefab 路径
            /// </summary>
            public string Path { get; private set; }
            /// <summary>
            /// PLC参数
            /// </summary>
            public object[] PLC_Params { get; private set; }

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="type"></param>
            /// <param name="path"></param>
            /// <param name="_params"></param>
            public PLC_InfoData(PLC_Type type, string path, params object[] _params)
            {
                this.PLC_Type = type;
                this.ScriptType = PLC_Define.GetPLC_Script(type);
                this.Path = path;
                this.PLC_Params = _params;
            }
        }

        /// <summary>
        /// 打开界面的字典
        /// </summary>
        private Dictionary<PLC_Type, GameObject> DictOpenPLCs = null;

        /// <summary>
        /// 准备打开界面的栈
        /// </summary>
        private Stack<PLC_InfoData> StackOpenPLCs = null;

        /// <summary>
        /// Init this Singleton.
        /// </summary>
        protected override void Init()
        {
            DictOpenPLCs = new Dictionary<PLC_Type, GameObject>();
            StackOpenPLCs = new Stack<PLC_InfoData>();
        }

        /// <summary>
        /// 判断是否打开
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsOpened(PLC_Type type)
        {
            if (DictOpenPLCs.ContainsKey(type))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取界面对象
        /// </summary>
        /// <param name="type">界面类型</param>
        /// <returns></returns>
        public GameObject GetPLC_Object(PLC_Type type)
        {
            GameObject retObj = null;
            if (!DictOpenPLCs.TryGetValue(type, out retObj))
                Debug.LogError("DictOpenPLCs TryGetValue Failure!PLC_Type: " + type.ToString());
            return retObj;
        }

        /// <summary>
        /// 获取界面对象脚本组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public T GetPLC<T>(PLC_Type type) where T : PLC_Base
        {
            GameObject retObj = GetPLC_Object(type);
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
        /// <param name="PLC_Types">界面类型数组</param>
        public void OpenPLC(PLC_Type[] PLC_Types)
        {
            OpenPLC(false, PLC_Types, null);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="PLC_Type">界面类型</param>
        /// <param name="PLCObjParams">界面参数</param>
        public void OpenPLC(PLC_Type PLC_Type, params object[] PLCObjParams)
        {
            PLC_Type[] PLC_Types = new PLC_Type[1];
            PLC_Types[0] = PLC_Type;
            OpenPLC(false, PLC_Types, PLCObjParams);
        }

        /// <summary>
        /// 打开界面，关闭其他界面
        /// </summary>
        /// <param name="PLC_Types">界面类型数组</param>
        public void OpenPLC_CloseOthers(PLC_Type[] PLC_Types)
        {
            OpenPLC(true, PLC_Types, null);
        }

        /// <summary>
        /// 打开界面，关闭其他界面
        /// </summary>
        /// <param name="PLC_Type">界面类型</param>
        /// <param name="PLCObjParams">界面参数</param>
        public void OpenPLC_CloseOthers(PLC_Type PLC_Type, params object[] PLCObjParams)
        {
            PLC_Type[] PLC_Types = new PLC_Type[1];
            PLC_Types[0] = PLC_Type;
            OpenPLC(true, PLC_Types, PLCObjParams);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="isCloseOthers">是否关闭其他PLC</param>
        /// <param name="PLC_Types">PLC类型数组</param>
        /// <param name="PLCParams">参数</param>
        private void OpenPLC(bool isCloseOthers, PLC_Type[] PLC_Types, params object[] PLCParams)
        {
            // Close Others PLC.
            if (isCloseOthers)
            {
                ClosePLCAll();
            }

            // push PLC_Types in Stack.
            for (int i = 0; i < PLC_Types.Length; i++)
            {
                PLC_Type PLC_Type = PLC_Types[i];
                if (!DictOpenPLCs.ContainsKey(PLC_Type))
                {
                    string _path = PLC_Define.GetPLC_PrefabPath(PLC_Type);
                    PLC_InfoData PLCInfoData = new PLC_InfoData(PLC_Type, _path, PLCParams);
                    StackOpenPLCs.Push(PLCInfoData);
                }
            }

            // Open PLC.
            if (StackOpenPLCs.Count > 0)
            {
                CoroutineManager.Instance.StartCoroutine(AsyncLoadData());
            }
        }

        private IEnumerator AsyncLoadData()
        {
            PLC_InfoData PLCInfoData = null;
            UnityEngine.Object _prefabObj = null;
            GameObject PLCObject = null;

            if (StackOpenPLCs != null && StackOpenPLCs.Count > 0)
            {
                do
                {
                    PLCInfoData = StackOpenPLCs.Pop();
                    AsyncLoadAssetOperation async = Assets.LoadAssetAsync<GameObject>(PLCInfoData.Path);
                    if (async != null)
                    {
                        _prefabObj = async.GetAsset<GameObject>();
                    }
                    if (_prefabObj != null)
                    {
                        PLCObject = MonoBehaviour.Instantiate(_prefabObj) as GameObject;
                        PLC_Base _basePLC = PLCObject.GetComponent<PLC_Base>();
                        if (null == _basePLC)
                        {
                            _basePLC = PLCObject.AddComponent(PLCInfoData.ScriptType) as PLC_Base;
                        }
                        if (null != _basePLC)
                        {
                            _basePLC.SetPLC_WhenOpening(PLCInfoData.PLC_Params);
                        }
                        DictOpenPLCs.Add(PLCInfoData.PLC_Type, PLCObject);
                    }
                } while (StackOpenPLCs.Count > 0);
            }
            yield return 0;
        }
        #endregion

        #region 关闭界面
        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="PLC_Type">界面类型</param>
        public void ClosePLC(PLC_Type PLC_Type)
        {
            GameObject PLCObj = null;
            if (!DictOpenPLCs.TryGetValue(PLC_Type, out PLCObj))
            {
                Debug.Log("dicOpenPLCs TryGetValue Failure! PLC_Type :" + PLC_Type.ToString());
                return;
            }
            ClosePLC(PLC_Type, PLCObj);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="PLC_Types">界面类型数组</param>
        public void ClosePLC(PLC_Type[] PLC_Types)
        {
            for (int i = 0; i < PLC_Types.Length; i++)
            {
                ClosePLC(PLC_Types[i]);
            }
        }

        /// <summary>
        /// 关闭所有PLC界面
        /// </summary>
        public void ClosePLCAll()
        {
            List<PLC_Type> _keyList = new List<PLC_Type>(DictOpenPLCs.Keys);
            foreach (PLC_Type PLC_Type in _keyList)
            {
                GameObject PLCObj = DictOpenPLCs[PLC_Type];
                ClosePLC(PLC_Type, PLCObj);
            }
            DictOpenPLCs.Clear();
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="PLC_Type">界面类型</param>
        /// <param name="PLCObj">界面对象</param>
        private void ClosePLC(PLC_Type PLC_Type, GameObject PLCObj)
        {
            if (PLCObj == null)
            {
                DictOpenPLCs.Remove(PLC_Type);
            }
            else
            {
                PLC_Base _basePLC = PLCObj.GetComponent<PLC_Base>();
                if (_basePLC != null)
                {
                    _basePLC.StateChanged += ClosePLCHandler;
                    _basePLC.Release();
                }
                else
                {
                    //GameObject.Destroy(PLCObj);
                    DictOpenPLCs.Remove(PLC_Type);
                }
            }
        }

        /// <summary>
        /// 关闭PLC后的操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newState"></param>
        /// <param name="oldState"></param>
        void ClosePLCHandler(object sender, EnumObjectState newState, EnumObjectState oldState)
        {
            if (newState == EnumObjectState.Closing)
            {
                PLC_Base basePLC = sender as PLC_Base;
                DictOpenPLCs.Remove(basePLC.GetPLC_Type());
                basePLC.StateChanged -= ClosePLCHandler;
            }
        }
        #endregion
    }
}
