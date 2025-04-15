using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.UI
{
    public class CultureMediumPreparationScene : ProductionSimulationScene
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
            //SceneParam param = SceneLoader.Instance.GetSceneParam(SceneType.CultureMediumPreparationScene);
            //if (param is ProductionSimulationSceneInfo)
            //{
            //    Examining = false;
            //    SceneInfo = param as ProductionSimulationSceneInfo;
            //    StageStyle = SceneInfo.StageStyle;
            //    StageType = SceneInfo.SelectStage;
            //    ProcedureType = SceneInfo.SelectProcedure;
            //    ProductionMode = SceneInfo.SelectMode;
            //    FaultID = SceneInfo.FaultID;
            //}
            //else if (param is StageSceneExamInfo)
            //{
            //    Examining = true;
            //    SceneExamInfo = param as StageSceneExamInfo;
            //    StageStyle = SceneExamInfo.StageStyle;
            //    ExamTransmitInfo = SceneExamInfo.ExamTransmitInfo;
            //    StageType = SceneExamInfo.SelectStage;
            //    ProcedureType = SceneExamInfo.SelectProcedure;
            //    ProductionMode = SceneExamInfo.SelectMode;
            //    FaultID = SceneExamInfo.FaultID;
            //}

            StageStyle = StageStyle.Standard;
            StageType = StageType.CultureMediumPreparationStage;
            ProcedureType = ProcedureType.Operate;
            ProductionMode = ProductionMode.Study;
        }
    }
}
