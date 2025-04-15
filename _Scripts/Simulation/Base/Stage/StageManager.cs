using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework
{
    /// <summary>
    /// 工段管理类
    /// </summary>
    public class StageManager : Singleton<StageManager>
    {
        /// <summary>
        /// 工段字典
        /// </summary>
        private Dictionary<StageType, BaseStage> dicStages = null;

        protected override void Init()
        {
            base.Init();
            dicStages = new Dictionary<StageType, BaseStage>();
        }

        #region Get Stage
        /// <summary>
        /// get module by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BaseStage GetStage(StageType stageType)
        {
            if (dicStages.ContainsKey(stageType))
                return dicStages[stageType];
            return null;
        }
        #endregion

        #region Load Stage
        /// <summary>
        /// 加载工段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">流程类型</param>
        /// <param name="mode">生产模式</param>
        public void Load(StageType stageType, ProductionMode mode, ProcedureType procedureType)
        {
            if (!dicStages.ContainsKey(stageType))
            {
                Type t = StageDefine.GetStageScript(stageType);
                BaseStage stage = Activator.CreateInstance(t) as BaseStage;
                stage.Initialize(stageType, mode, procedureType);
                dicStages.Add(stageType, stage);
            }
        }

        /// <summary>
        /// 加载工段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">流程类型</param>
        /// <param name="mode">生产模式</param>
        public void Load(StageType stageType, ProductionMode mode, string faultID)
        {
            if (!dicStages.ContainsKey(stageType))
            {
                Type t = StageDefine.GetStageScript(stageType);
                BaseStage stage = Activator.CreateInstance(t) as BaseStage;
                stage.Initialize(stageType, mode, faultID);
                dicStages.Add(stageType, stage);
            }
        }
        #endregion



        #region Release Stage
        /// <summary>
        /// 卸载工段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Unload(StageType stageType)
        {
            if (dicStages.ContainsKey(stageType))
            {
                BaseStage stage = dicStages[stageType];
                stage.Release();
                dicStages.Remove(stageType);
            }
        }

        /// <summary>
        /// 释放所有工段
        /// </summary>
        public void UnloadAll()
        {
            List<StageType> _keyList = new List<StageType>(dicStages.Keys);
            for (int i = 0; i < _keyList.Count; i++)
            {
                Unload(_keyList[i]);
            }
            dicStages.Clear();
        }
        #endregion
    }
}
