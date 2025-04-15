using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;
using XFramework.UI;

namespace XFramework.UI
{
    public class PanelManager : Singleton<PanelManager>
    {
        /// <summary>
        /// Panel信息数据类
        /// </summary>
        public class PanelInfoData
        {
            /// <summary>
            /// Panel类型
            /// </summary>
            public EnumPanelType PanelType { get; private set; }

            public Type ScriptType { get; private set; }
            /// <summary>
            /// Panel prefab 路径
            /// </summary>
            public string Path { get; private set; }
            /// <summary>
            /// Panel参数
            /// </summary>
            public object[] PanelParams { get; private set; }
            /// <summary>
            /// Panel父物体
            /// </summary>
            public Transform Parent { get; set; }

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="type"></param>
            /// <param name="path"></param>
            /// <param name="_params"></param>
            public PanelInfoData(Transform parent, EnumPanelType type, string path, params object[] _params)
            {
                this.Parent = parent;
                this.PanelType = type;
                this.ScriptType = PanelDefine.GetPanelScript(type);
                this.Path = path;
                this.PanelParams = _params;
            }
        }

        /// <summary>
        /// 打开面板的字典
        /// </summary>
        private Dictionary<EnumPanelType, GameObject> DictOpenPanels = null;

        /// <summary>
        /// 准备打开面板的栈
        /// </summary>
        private Stack<PanelInfoData> StackOpenPanels = null;

        /// <summary>
        /// 面板容器字典 （注意试用）
        /// </summary>
        public Dictionary<PanelContainerType, Transform> PanelContainers = null;

        /// <summary>
        /// Init this Singleton.
        /// </summary>
        protected override void Init()
        {
            DictOpenPanels = new Dictionary<EnumPanelType, GameObject>();
            StackOpenPanels = new Stack<PanelInfoData>();
            PanelContainers = new Dictionary<PanelContainerType, Transform>();
        }

        /// <summary>
        /// 获取面板对象
        /// </summary>
        /// <param name="type">面板类型</param>
        /// <returns></returns>
        public GameObject GetPanelObject(EnumPanelType type)
        {
            GameObject retObj = null;
            if (!DictOpenPanels.TryGetValue(type, out retObj))
                throw new ApplicationException("DictOpenPanels TryGetValue Failure!_PanelType: " + type.ToString());
            return retObj;
        }

        /// <summary>
        /// 获取面板对象脚本组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public T GetPanel<T>(EnumPanelType type) where T : AbstractPanel
        {
            GameObject retObj = GetPanelObject(type);
            if (retObj != null)
            {
                return retObj.GetComponent<T>();
            }
            return null;
        }


        #region 打开面板
        /// <summary>
        /// 打开面板
        /// </summary>
        /// <param name="PanelTypes">面板类型数组</param>
        public void OpenPanel(Transform parent, EnumPanelType[] PanelTypes)
        {
            OpenPanel(parent, false, PanelTypes, null);
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        /// <param name="PanelType">面板类型</param>
        /// <param name="PanelObjParams">面板参数</param>
        public void OpenPanel(PanelContainerType panelContainerType, EnumPanelType PanelType, params object[] PanelObjParams)
        {
            if (PanelContainers.ContainsKey(panelContainerType))
            {
                Transform parent = PanelContainers[panelContainerType];
                EnumPanelType[] PanelTypes = new EnumPanelType[1];
                PanelTypes[0] = PanelType;
                OpenPanel(parent, false, PanelTypes, PanelObjParams);
            }
            else
            {
                Debug.LogErrorFormat("PanelContainers not contain PanelContainerType : {}", panelContainerType);
            }
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        /// <param name="PanelType">面板类型</param>
        /// <param name="PanelObjParams">面板参数</param>
        public void OpenPanel(Transform parent, EnumPanelType PanelType, params object[] PanelObjParams)
        {
            EnumPanelType[] PanelTypes = new EnumPanelType[1];
            PanelTypes[0] = PanelType;
            OpenPanel(parent, false, PanelTypes, PanelObjParams);
        }

        /// <summary>
        /// 打开面板，关闭其他面板
        /// </summary>
        /// <param name="PanelTypes">面板类型数组</param>
        public void OpenPanelCloseOthers(Transform parent, EnumPanelType[] PanelTypes)
        {
            OpenPanel(parent, true, PanelTypes, null);
        }

        /// <summary>
        /// 打开面板，关闭其他面板
        /// </summary>
        /// <param name="PanelType">面板类型</param>
        /// <param name="PanelObjParams">面板参数</param>
        public void OpenPanelCloseOthers(PanelContainerType panelContainerType, EnumPanelType PanelType, params object[] PanelObjParams)
        {
            if (PanelContainers.ContainsKey(panelContainerType))
            {
                Transform parent = PanelContainers[panelContainerType];
                EnumPanelType[] PanelTypes = new EnumPanelType[1];
                PanelTypes[0] = PanelType;
                OpenPanel(parent, true, PanelTypes, PanelObjParams);
            }
            else
            {
                Debug.LogErrorFormat("PanelContainers not contain PanelContainerType : {}", panelContainerType);
            }
        }

        /// <summary>
        /// 打开面板，关闭其他面板
        /// </summary>
        /// <param name="PanelType">面板类型</param>
        /// <param name="PanelObjParams">面板参数</param>
        public void OpenPanelCloseOthers(Transform parent, EnumPanelType PanelType, params object[] PanelObjParams)
        {
            EnumPanelType[] PanelTypes = new EnumPanelType[1];
            PanelTypes[0] = PanelType;
            OpenPanel(parent, true, PanelTypes, PanelObjParams);
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        /// <param name="isCloseOthers">是否关闭其他Panel</param>
        /// <param name="PanelTypes">Panel类型数组</param>
        /// <param name="PanelParams">参数</param>
        private void OpenPanel(Transform parent, bool isCloseOthers, EnumPanelType[] PanelTypes, params object[] PanelParams)
        {
            // Close Others Panel.
            if (isCloseOthers)
            {
                ClosePanelAll();
            }

            // push _PanelTypes in Stack.
            for (int i = 0; i < PanelTypes.Length; i++)
            {
                EnumPanelType _PanelType = PanelTypes[i];
                if (!DictOpenPanels.ContainsKey(_PanelType))
                {
                    string _path = PanelDefine.GetPanelPrefabPath(_PanelType);
                    PanelInfoData PanelInfoData = new PanelInfoData(parent, _PanelType, _path, PanelParams);
                    StackOpenPanels.Push(PanelInfoData);
                }
            }

            // Open Panel.
            if (StackOpenPanels.Count > 0)
            {
                CoroutineManager.Instance.StartCoroutine(AsyncLoadData());
            }
        }

        private IEnumerator AsyncLoadData()
        {
            PanelInfoData _PanelInfoData = null;
            UnityEngine.Object _prefabObj = null;
            GameObject _PanelObject = null;

            if (StackOpenPanels != null && StackOpenPanels.Count > 0)
            {
                do
                {
                    _PanelInfoData = StackOpenPanels.Pop();
                    //_prefabObj = Resources.Load(_PanelInfoData.Path);
                    AsyncLoadAssetOperation async = Assets.LoadAssetAsync<GameObject>(_PanelInfoData.Path);
                    yield return async;
                    _prefabObj = async.GetAsset<GameObject>();

                    if (_prefabObj != null)
                    {
                        _PanelObject = MonoBehaviour.Instantiate(_prefabObj) as GameObject;
                        _PanelObject.transform.SetParent(_PanelInfoData.Parent, false);
                        _PanelObject.layer = _PanelInfoData.Parent.gameObject.layer;
                        AbstractPanel abstractPanel = _PanelObject.GetComponent<AbstractPanel>();
                        if (null == abstractPanel)
                        {
                            abstractPanel = _PanelObject.AddComponent(_PanelInfoData.ScriptType) as AbstractPanel;
                        }
                        if (null != abstractPanel)
                        {
                            yield return 0;
                            abstractPanel.SetPanelWhenOpening(_PanelInfoData.PanelParams);
                            abstractPanel.Parent = _PanelInfoData.Parent;
                            abstractPanel.Parent.localPosition = Vector3.zero;
                        }
                        DictOpenPanels.Add(_PanelInfoData.PanelType, _PanelObject);
                    }
                } while (StackOpenPanels.Count > 0);
            }
            yield return 0;
        }
        #endregion

        #region 关闭面板
        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="PanelType">面板类型</param>
        public void ClosePanel(EnumPanelType PanelType)
        {
            GameObject PanelObj = null;
            if (!DictOpenPanels.TryGetValue(PanelType, out PanelObj))
            {
                Debug.Log("dicOpenPanels TryGetValue Failure! _PanelType :" + PanelType.ToString());
                return;
            }
            ClosePanel(PanelType, PanelObj);
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="PanelTypes">面板类型数组</param>
        public void ClosePanel(EnumPanelType[] PanelTypes)
        {
            for (int i = 0; i < PanelTypes.Length; i++)
            {
                ClosePanel(PanelTypes[i]);
            }
        }

        /// <summary>
        /// 关闭所有Panel面板
        /// </summary>
        public void ClosePanelAll()
        {
            List<EnumPanelType> _keyList = new List<EnumPanelType>(DictOpenPanels.Keys);
            foreach (EnumPanelType _PanelType in _keyList)
            {
                GameObject _PanelObj = DictOpenPanels[_PanelType];
                ClosePanel(_PanelType, _PanelObj);
            }
            DictOpenPanels.Clear();
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="PanelType">面板类型</param>
        /// <param name="PanelObj">面板对象</param>
        private void ClosePanel(EnumPanelType PanelType, GameObject PanelObj)
        {
            if (PanelObj == null)
            {
                DictOpenPanels.Remove(PanelType);
            }
            else
            {
                AbstractPanel abstractPanel = PanelObj.GetComponent<AbstractPanel>();
                if (abstractPanel != null)
                {
                    abstractPanel.StateChanged += ClosePanelHandler;
                    abstractPanel.Release();
                }
                else
                {
                    //GameObject.Destroy(PanelObj);
                    DictOpenPanels.Remove(PanelType);
                }
            }
        }

        /// <summary>
        /// 关闭Panel后的操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newState"></param>
        /// <param name="oldState"></param>
        void ClosePanelHandler(object sender, EnumObjectState newState, EnumObjectState oldState)
        {
            if (newState == EnumObjectState.Closing)
            {
                AbstractPanel abstractPanel = sender as AbstractPanel;
                DictOpenPanels.Remove(abstractPanel.GetPanelType());
                abstractPanel.StateChanged -= ClosePanelHandler;
            }
        }
        #endregion
    }
}
