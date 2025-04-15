using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 栈管理ui
    /// </summary>
    public class RamingUIManager:Singleton<RamingUIManager>
    {
        class UIInfoData
        {
            public RamingUI UIType { get; private set; }
            public object[] UIParams { get; private set; }
            public UIInfoData(RamingUI _uiType, params object[] _uiParams)
            {
                this.UIType = _uiType;
                this.UIParams = _uiParams;

            }
        }

        //private Stack<BaseContext> _contextStack = new Stack<BaseContext>();
        private Stack<UIInfoData> _StackOpingUI = new Stack<UIInfoData>();

        /// <summary>
        /// 打开的窗口字典
        /// </summary>
        private Dictionary<RamingUI, BaseRamingUI> uiDic ;

        private List<RamingUI> buffList;

        protected override void Init()
        {
            base.Init();
            uiDic = new Dictionary<RamingUI, BaseRamingUI>();
            buffList = new List<RamingUI>();
        }

        public void OpenPanel(RamingUI uiType)
        {
            if (GetCurrentUI() != null && GetCurrentUI().UIType == uiType)
                return;
            CheakPanel(uiType);

            if(!uiDic.ContainsKey(uiType))
            {
                _StackOpingUI.Push(new UIInfoData(uiType));

            }

            // Open UI.
            if (_StackOpingUI.Count > 0)
            {
                CoroutineManager.Instance.StartCoroutine(AsyncLoadData());
            }
        }

        public void OpenPanel(RamingUI uiType, params object[] _uiParams)
        {
         
            if (GetCurrentUI()!=null&& GetCurrentUI().UIType == uiType)
                return;
            CheakPanel(uiType);

            if (!uiDic.ContainsKey(uiType))
            {
                _StackOpingUI.Push(new UIInfoData(uiType,_uiParams));

            }

            // Open UI.
            if (_StackOpingUI.Count > 0)
            {
                CoroutineManager.Instance.StartCoroutine(AsyncLoadData());
            }
        }
        public void CheakPanel(RamingUI uiType)
        {

            BaseRamingUI _UIScript = null;
            _UIScript = RamingUiInfo.Instance.GetUiObject(uiType);

            if (_UIScript == null)
                return;
            if (_UIScript.ShowMode == UIWindowShowMode.DoNothing)
            {
                return;
            }
            else if (_UIScript.ShowMode == UIWindowShowMode.HideOther)
            {
                ///判断是否存在 hideOther类型的 上下文 关闭它
                ///
                RamingUI _typt = uiType;

                foreach (KeyValuePair<RamingUI, BaseRamingUI> kvp in uiDic)
                {
                    BaseRamingUI _ramingUI = kvp.Value;
                    RamingUI _type = kvp.Key;

                    if (_ramingUI.ShowMode == UIWindowShowMode.HideOther)
                    {
                        ClosePanel(_type, _ramingUI);
                    }
                }
                ClearBuff();
            }
        }
        private IEnumerator<int> AsyncLoadData()
        {
            UIInfoData _uiInfoData = null;
            BaseRamingUI _UIScript = null;
        
            if (_StackOpingUI != null && _StackOpingUI.Count > 0)
            {
                _uiInfoData = _StackOpingUI.Pop();

                _UIScript = RamingUiInfo.Instance.GetUiObject(_uiInfoData.UIType);
                if (_UIScript != null)
                {
                    if(_uiInfoData.UIParams.Length==0)
                    {
                        ///显示试图
                        _UIScript.Show(null);
                        uiDic.Add(_uiInfoData.UIType, _UIScript);

                    }
                    else
                    {
                        BaseUIArgs param = _uiInfoData.UIParams[0] as BaseUIArgs;
                        ///显示试图
                        _UIScript.Show(param);
                        uiDic.Add(_uiInfoData.UIType, _UIScript);
                    }
                   
                }
            }
            yield return 0;
        }
        private  UIInfoData GetCurrentUI()
        {
            return _StackOpingUI.Count > 0 ? _StackOpingUI.Pop() : null; 
        }

        public void ClosePanel(RamingUI uiType,BaseRamingUI _ui)
        {
            if (_ui == null)
            {
                //uiDic.Remove(uiType);
                buffList.Add(uiType);
            }
            else
            {
                _ui.Hide(new BaseUIArgs());
                // uiDic.Remove(uiType);
                buffList.Add(uiType);
            }
        }

     
        public void ClosePanel(RamingUI uiType)
        {
            BaseRamingUI _UIScript = null;
            _UIScript = RamingUiInfo.Instance.GetUiObject(uiType);
            ClosePanel(uiType, _UIScript);
            ClearBuff();

        }

        public void CloseAllPanel()
        {
            List<RamingUI> uiList = new List<RamingUI>(uiDic.Keys);

            foreach (RamingUI type in uiList)
            {
                BaseRamingUI script = uiDic[type];
                ClosePanel(type,script);
            }
            ClearBuff();
            MonoBehaviour. print("移除所有");
        }


        private void ClearBuff()
        {
            for (int i = 0; i < buffList.Count; i++)
            {
                RamingUI type = buffList[i];
                uiDic.Remove(type);
            }
            buffList.Clear();
        }
       
    }
}
