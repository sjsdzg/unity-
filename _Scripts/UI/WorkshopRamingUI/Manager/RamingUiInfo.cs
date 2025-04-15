using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;

namespace XFramework.UI
{

    /// <summary>
    /// 漫游数据类
    /// </summary>
    public class RamingUiInfo:Singleton<RamingUiInfo>
    {

        public Dictionary<RamingUI, BaseRamingUI> objDic;

        protected override void Init()
        {
            base.Init();
            objDic = new Dictionary<RamingUI, BaseRamingUI>();


        }

        public void RegisterUI(RamingUI type, BaseRamingUI ui)
        {
            if (!objDic.ContainsKey(type))
            {
                objDic.Add(type,ui);
            }
        }

        public BaseRamingUI GetUiObject(RamingUI type)
        {
            BaseRamingUI ui = null;

            if(objDic.TryGetValue(type,out ui))
            {
                return ui;
            }
            return null;
        }
        public void RemoveAll()
        {
            objDic.Clear();
        }
    }
}
