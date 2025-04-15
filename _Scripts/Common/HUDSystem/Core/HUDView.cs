using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    /// <summary>
    /// HUD视图
    /// </summary>
    public abstract class HUDView : MonoBehaviour
    {
        /// <summary>
        /// 获取Id
        /// </summary>
        public int Id
        {
            get { return GetInstanceID(); }
        }

        /// <summary>
        /// Target
        /// </summary>
        public Transform m_Target;

        private bool m_OnScreen;
        /// <summary>
        /// 是否在屏幕内
        /// </summary>
        public bool OnScreen
        {
            get { return m_OnScreen; }
            set
            {
                if (m_OnScreen != value)
                {
                    Transition(value);
                }

                m_OnScreen = value;

                if (m_OnScreen)
                {
                    OnScreenHandler();
                }
                else
                {
                    OffScreenHandler();
                }
            }
        }

        /// <summary>
        /// HUDInfo
        /// </summary>
        public abstract HUDInfo HUDInfo { get; }

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize(HUDInfo hudInfo)
        {
            m_Target = hudInfo.m_Target;
        }

        /// <summary>
        /// 过渡(屏幕内<=>屏幕外)
        /// </summary>
        /// <param name="onScreenNextValue"></param>
        protected virtual void Transition(bool onScreenNextValue)
        {
            
        }

        /// <summary>
        /// 在屏幕内显示()
        /// </summary>
        protected virtual void OnScreenHandler()
        {

        }

        /// <summary>
        /// 在屏幕外显示
        /// </summary>
        protected virtual void OffScreenHandler()
        {

        }

        /// <summary>
        /// 更新视图
        /// </summary>
        public virtual void UpdateView()
        {

        }
    }
}
