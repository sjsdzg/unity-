using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
	public class BaseModule
    {
        #region Module State
        private EnumObjectState state = EnumObjectState.Initial;
        /// <summary>
        /// 模块加载状态
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

        #region 模块状态改变事件
        public event StateChangedEventHandler StateChanged;
        #endregion

        protected virtual void OnStateChanged(EnumObjectState newState, EnumObjectState oldState) 
        {
            
        }

        /// <summary>
        /// 模块加载
        /// </summary>
        public void Load()
        {
            if (State != EnumObjectState.Initial)
                return;
            State = EnumObjectState.Loading;
            OnLoad();
            State = EnumObjectState.Ready;
        }

        protected virtual void OnLoad() 
        {

        }

        /// <summary>
        /// 模块释放
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

	}
}
