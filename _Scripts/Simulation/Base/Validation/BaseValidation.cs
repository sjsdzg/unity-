using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.Simulation
{
    /// <summary>
    /// 验证基类
    /// </summary>
    public abstract class BaseValidation
    {
        #region Module State
        private EnumObjectState state = EnumObjectState.Initial;
        /// <summary>
        /// 验证加载状态
        /// </summary>
        public EnumObjectState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    EnumObjectState oldState = state;
                    state = value;
                    if (null != StateChanged)
                    {
                        StateChanged(this, state, oldState);
                    }
                    OnStateChanged(state, oldState);
                }
            }
        }
        #endregion

        #region 验证状态改变事件
        public event StateChangedEventHandler StateChanged;
        #endregion

        protected virtual void OnStateChanged(EnumObjectState newState, EnumObjectState oldState)
        {

        }

        private ProductionMode m_ProductMode;
        /// <summary>
        /// 生产模式
        /// </summary>
        public ProductionMode ProductMode
        {
            get { return m_ProductMode; }
        }

        private ValidationType m_ValidationType;
        /// <summary>
        /// 验证类型
        /// </summary>
        public ValidationType ValidationType
        {
            get { return m_ValidationType; }
        }

        /// <summary>
        /// 流程
        /// </summary>
        public abstract Procedure Procedure { get; set; }

        /// <summary>
        /// 验证初始化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mode"></param>
        public void Initialize(ValidationType validationType, ProductionMode productMode)
        {
            if (State != EnumObjectState.Initial)
                return;
            State = EnumObjectState.Loading;

            m_ValidationType = validationType;
            m_ProductMode = productMode;

            OnInitialize();

            State = EnumObjectState.Ready;
        }

        protected virtual void OnInitialize()
        {

        }

        /// <summary>
        /// 验证释放
        /// </summary>
        public void Release()
        {
            if (State != EnumObjectState.Disabled)
            {
                State = EnumObjectState.Closing;
                OnRelease();
                State = EnumObjectState.Disabled;
            }
        }

        protected virtual void OnRelease()
        {

        }

        ///// <summary>
        ///// 处理文件结果
        ///// </summary>
        //public virtual void HandleFileResult(FileResult result)
        //{

        //}

        ///// <summary>
        ///// 加工文件数据
        ///// </summary>
        ///// <param name="item"></param>
        //public virtual void ProcessFileData(Document item)
        //{

        //}

        ///// <summary>
        ///// 提交考核内容
        ///// </summary>
        //public virtual void SumbitExamine()
        //{

        //}
    }
}
