using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Module;
using XFramework.Core;
using XFramework.UI;

namespace XFramework.Simulation
{
    /// <summary>
    /// 工段基类
    /// </summary>
    public abstract class BaseStage
    {
        #region Module State
        private EnumObjectState state = EnumObjectState.Initial;
        /// <summary>
        /// 工段加载状态
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

        #region 工段状态改变事件
        public event StateChangedEventHandler StateChanged;
        #endregion

        protected virtual void OnStateChanged(EnumObjectState newState, EnumObjectState oldState)
        {

        }

        private StageType m_StageType;
        /// <summary>
        /// 工段类型
        /// </summary>
        public StageType StageType
        {
            get { return m_StageType; }
            private set { m_StageType = value; }
        }

        private ProductionMode m_ProductMode;
        /// <summary>
        /// 生产模式
        /// </summary>
        public ProductionMode ProductMode
        {
            get { return m_ProductMode; }
            private set { m_ProductMode = value; }
        }

        private ProcedureType m_ProcedureType;
        /// <summary>
        /// 流程类型
        /// </summary>
        public ProcedureType ProcedureType
        {
            get { return m_ProcedureType; }
            private set { m_ProcedureType = value; }
        }

        private StageStyle m_StageStyle = StageStyle.Standard;
        /// <summary>
        /// 工段风格
        /// </summary>
        public StageStyle StageStyle
        {
            get { return m_StageStyle; }
            private set { m_StageStyle = value; }
        }

        private string m_FaultID;
        /// <summary>
        /// 故障ID
        /// </summary>
        public string FaultID
        {
            get { return m_FaultID; }
            set { m_FaultID = value; }
        }


        /// <summary>
        /// 工段初始化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mode"></param>
        public void Initialize(StageType stageType, ProductionMode productMode, ProcedureType procedureType)
        {
            if (State != EnumObjectState.Initial)
                return;

            State = EnumObjectState.Loading;
            StageStyle = StageStyle.Standard;
            m_StageType = stageType;
            m_ProductMode = productMode;
            m_ProcedureType = procedureType;
            OnInitialize();
            State = EnumObjectState.Ready;
        }

        public void Initialize(StageType stageType, ProductionMode productMode, string faultID)
        {
            if (State != EnumObjectState.Initial)
                return;
            
            State = EnumObjectState.Loading;
            StageStyle = StageStyle.Fault;
            m_StageType = stageType;
            m_ProductMode = productMode;
            FaultID = faultID;
            OnInitialize();
            State = EnumObjectState.Ready;
        }

        protected virtual void OnInitialize()
        {

        }

        /// <summary>
        /// 工段释放
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

        /// <summary>
        /// 获取考核内容
        /// </summary>
        /// <returns></returns>
        public abstract AssessmentGrade GetAssessmentGrade();

        public virtual List<CheckQuestion> GetCheckQuestionList()
        {
            return null;

        }

    }
}
