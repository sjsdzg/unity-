using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Core;
using XFramework.Module;
using XFramework.PLC;
using XFramework.Simulation;

namespace XFramework.UI
{
    public class CellCultivateSimulationScene : ProductionSimulationScene
    {

        /// <summary>
        /// 解析场景参数
        /// </summary>
        protected override void ParseSceneParam()
        {
            base.ParseSceneParam();
            SceneInfo = new ProductionSimulationSceneInfo();
            SceneExamInfo = new StageSceneExamInfo();
            //解析
            SceneParam param = SceneLoader.Instance.GetSceneParam(SceneType.CellCultivateStageScene);
            if (param is ProductionSimulationSceneInfo)
            {
                Examining = false;
                SceneInfo = param as ProductionSimulationSceneInfo;
                StageStyle = SceneInfo.StageStyle;
                StageType = SceneInfo.SelectStage;
                ProcedureType = SceneInfo.SelectProcedure;
                ProductionMode = SceneInfo.SelectMode;
                FaultID = SceneInfo.FaultID;
            }
            else if (param is StageSceneExamInfo)
            {
                Examining = true;
                SceneExamInfo = param as StageSceneExamInfo;
                StageStyle = SceneExamInfo.StageStyle;
                ExamTransmitInfo = SceneExamInfo.ExamTransmitInfo;
                StageType = SceneExamInfo.SelectStage;
                ProcedureType = SceneExamInfo.SelectProcedure;
                ProductionMode = SceneExamInfo.SelectMode;
                FaultID = SceneExamInfo.FaultID;
            }

            //StageStyle = StageStyle.Standard;
            //StageType = StageType.CellCultivateStage;
            //ProcedureType = ProcedureType.Operate;
            //ProductionMode = ProductionMode.Study;
        }
    }
}
