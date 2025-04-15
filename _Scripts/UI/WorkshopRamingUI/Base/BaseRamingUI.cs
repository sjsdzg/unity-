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
    /// 漫游 ui类型
    /// </summary>
    public enum RamingUI
    {

        /// <summary>
        /// 总体介绍窗口
        /// </summary>
        Overall,

        /// <summary>
        /// 设备展示窗口
        /// </summary>
        Device,

        /// <summary>
        /// 介绍列表
        /// </summary>
        IntroduceTree,

        /// <summary>
        /// hud标签
        /// </summary>
        HUDLabel,
    }
    /// <summary>
    /// 显示方式
    /// </summary>
    public enum UIWindowShowMode
    {
        DoNothing,     //不受其他影响
        HideOther,     // 闭其他界面 收其他影响
                       //NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
                       // NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
    }
    /// <summary>
    /// 漫游ui
    /// </summary>
	public class BaseRamingUI : MonoBehaviour{
        /// <summary>
        /// 是否激活
        /// </summary>
        private bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (IsActive == value)
                    return;
                isActive = value;
                gameObject.SetActive(value);
            }
        }
        /// <summary>
        /// 显示类型
        /// </summary>
        public  UIWindowShowMode ShowMode;

        public BaseUIArgs param;
        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void Hide(BaseUIArgs args)
        {
            
        }
        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show(BaseUIArgs args)
        {
            param = args;
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        public virtual void  OnPlayMp3()
        {

        }
        public void SetUIWhenOpening(params object[] uiParams)
        {
            SetUI(uiParams);
            StartCoroutine(AsyncOnLoadData());
        }

        protected virtual void SetUI(params object[] uiParams)
        {
            
        }

        private IEnumerator AsyncOnLoadData()
        {
            yield return new WaitForEndOfFrame();
            this.OnLoadData();
        }

        protected virtual void OnLoadData()
        {

        }
    }



    ///// <summary>
    ///// 漫游 ui状态
    ///// </summary>
    //public abstract class BaseRamingUIState
    //{

    //    public abstract void Show(object msg);

    //    public abstract void Hide(object msg);

    //}
}