using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Common;
using XFramework.Core;
using XFramework.Core;
using XFramework.Module;
using XFramework.PLC;
using XFramework.Simulation;

namespace XFramework.UI
{
    /// <summary>
    /// 验证仿真启动
    /// </summary>
    public class ProductionSimulationScene : BaseScene
    {
        /// <summary>
        /// 场景信息
        /// </summary>
        public ProductionSimulationSceneInfo SceneInfo { get; set; }

        /// <summary>
        /// 考试场景信息
        /// </summary>
        public StageSceneExamInfo SceneExamInfo { get; set; }

        /// <summary>
        /// 是否正在处于考试中
        /// </summary>
        public bool Examining { get; set; }

        /// <summary>
        /// 考试传递的信息
        /// </summary>
        public ExamTransmitInfo ExamTransmitInfo { get; set; }

        public StageStyle StageStyle { get; set; }

        public StageType StageType { get; set; }

        public ProcedureType ProcedureType { get; set; }

        public ProductionMode ProductionMode { get; set; }

        public string FaultID { get; set; }

        /// <summary>
        /// 解析场景参数
        /// </summary>
        protected override void ParseSceneParam()
        {
            base.ParseSceneParam();
            SceneInfo = new ProductionSimulationSceneInfo();
            SceneExamInfo = new StageSceneExamInfo();
            //解析
            //SceneParam param = SceneLoader.Instance.GetSceneParam(SceneType.CellExpansionStageScene);
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

        protected override List<string> GetPreloadPaths()
        {
            List<string> paths = new List<string>();

            switch (StageStyle)
            {
                case StageStyle.Standard:
                    //加载ProductionSimulationModule
                    ModuleManager.Instance.Register<ProductionSimulationModule>();
                    ProductionSimulationModule standradModule = ModuleManager.Instance.GetModule<ProductionSimulationModule>();
                    standradModule.Init(StageType, ProcedureType);
                    //------------预加载资源----------------
                    //ProductionSimulationUI
                    paths.Add(UIDefine.GetUIPrefabPath(EnumUIType.ProductionSimulationUI));
                    //车间资源
                    string[] workshops = standradModule.GetProcedure().Workshops.Split('|');
                    foreach (string workshop in workshops)
                    {
                        EnumWorkshopType workshopType = (EnumWorkshopType)Enum.Parse(typeof(EnumWorkshopType), workshop);
                        paths.Add(WorkshopDefine.GetWorkshopPrefabPath(workshopType));
                    }
                    //人物资源
                    List<EntityBase> entities = standradModule.GetEntities();
                    foreach (EntityBase entity in entities)
                    {
                        paths.Add(CharacterDefine.GetCharacterPrefabPath(entity.Cleanliness, entity.Gender));
                    }
                    //------------预加载资源----------------
                    break;
                case StageStyle.Fault:
                    //加载FaultSimulationModule
                    ModuleManager.Instance.Register<FaultSimulationModule>();
                    FaultSimulationModule faultModule = ModuleManager.Instance.GetModule<FaultSimulationModule>();
                    faultModule.Init(StageType, FaultID);
                    //------------预加载资源----------------
                    //ProductionSimulationUI
                    paths.Add(UIDefine.GetUIPrefabPath(EnumUIType.FaultSimulationUI));
                    //车间资源
                    workshops = faultModule.GetFault().Workshops.Split('|');
                    foreach (string workshop in workshops)
                    {
                        EnumWorkshopType workshopType = (EnumWorkshopType)Enum.Parse(typeof(EnumWorkshopType), workshop);
                        paths.Add(WorkshopDefine.GetWorkshopPrefabPath(workshopType));
                    }
                    //人物资源
                    entities = faultModule.GetEntities();
                    foreach (EntityBase entity in entities)
                    {
                        paths.Add(CharacterDefine.GetCharacterPrefabPath(entity.Cleanliness, entity.Gender));
                    }
                    //------------预加载资源----------------
                    break;
                default:
                    break;
            }

            return paths;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            //加载
            StartCoroutine(Loading());
        }

        /// <summary>
        /// 启动过程
        /// </summary>
        IEnumerator Loading()
        {
            switch (StageStyle)
            {
                case StageStyle.Standard:
                    ProductionSimulationModule standradModule = ModuleManager.Instance.GetModule<ProductionSimulationModule>();
                    ScreenFade(StageDefine.GetStageName(standradModule.CurrentStageType) + " - " +
                    standradModule.GetProcedure().Name,() =>
                    {
                        EventDispatcher.ExecuteEvent("ProductionSimulationFaderComplete");
                    });
                    yield return new WaitForEndOfFrame();
                    //打开UI
                    UIManager.Instance.OpenUI(EnumUIType.ProductionSimulationUI, this);
                    yield return new WaitForSeconds(2f);
                    //加载工段
                    StageManager.Instance.Load(StageType, ProductionMode, ProcedureType);
                    yield return new WaitForEndOfFrame();
                    break;
                case StageStyle.Fault:
                    FaultSimulationModule faultModule = ModuleManager.Instance.GetModule<FaultSimulationModule>();
                    ScreenFade(StageDefine.GetStageName(faultModule.CurrentStageType) + "故障 - " + faultModule.GetFault().Name);
                    yield return new WaitForEndOfFrame();
                    //打开UI
                    UIManager.Instance.OpenUI(EnumUIType.FaultSimulationUI, this);
                    yield return new WaitForSeconds(2f);
                    //加载工段
                    StageManager.Instance.Load(StageType, ProductionMode, FaultID);
                    yield return new WaitForEndOfFrame();
                    break;
                default:
                    break;
            }
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            UIManager.Instance.CloseUI(EnumUIType.ProductionSimulationUI);
            UIManager.Instance.CloseUI(EnumUIType.FaultSimulationUI);
            EventDispatcher.UnregisterAllEvent();
            Messager.Instance.RemoveAllListener();
            ModuleManager.Instance.Unregister<ProductionSimulationModule>();
            ModuleManager.Instance.Unregister<FaultSimulationModule>();
            StageManager.Instance.Unload(StageType);
            WorkshopManager.Instance.UnloadWorkshopAll();
            PLCManager.Instance.ClosePLCAll();
            SmallActionManager.Instance.Release();
            CameraLookPointManager.Instance.Release();
            FocusManager.Instance.Release();
            LookPointManager.Instance.Release();
            DOTween.KillAll();
        }

        /// <summary>
        /// 屏幕过度
        /// </summary>
        /// <param name="content"></param>
        private void ScreenFade(string content)
        {
            ScreenFader.Instance
                .FadeIn(0).Pause()
                .SetText(content, 0)
                .FadeTextIn(0.5f).Pause(2.5f).FadeTextOut(0.5f)
                .FadeOut()
                .Execute();
        }
        /// <summary>
        /// 屏幕过度
        /// </summary>
        /// <param name="content"></param>
        private void ScreenFade(string content, UnityAction action = null)
        {
#if UNITY_WEBGL
            ScreenFader.Instance
                .FadeIn(0).Pause()
                .SetText(content, 0)
                .FadeTextIn(0.5f).Pause(4f).FadeTextOut(0.5f)
                .FadeOut()
                .OnCompleted(action)
                .Execute();
#else
            ScreenFader.Instance
                .FadeIn(0).Pause()
                .SetText(content, 0)
                .FadeTextIn(0.5f).Pause(2.5f).FadeTextOut(0.5f)
                .FadeOut()
                .OnCompleted(action)
                .Execute();
#endif
        }
    
}

    /// <summary>
    /// 工段主场景中的 数据类
    /// </summary>
    public class ProductionSimulationSceneInfo : SceneParam
    {
        /// <summary>
        /// 工段类型
        /// </summary>
        public StageType SelectStage { get; private set; }

        /// <summary>
        /// 选择的模式
        /// </summary>
        public ProductionMode SelectMode { get; private set; }

        /// <summary>
        /// 选择的流程
        /// </summary>
        public ProcedureType SelectProcedure { get; private set; }

        /// <summary>
        /// 故障Id
        /// </summary>
        public string FaultID { get; set; }

        /// <summary>
        /// 工段风格
        /// </summary>
        public StageStyle StageStyle { get; private set; }

        public ProductionSimulationSceneInfo()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stageType"></param>
        /// <param name="selectMode"></param>
        /// <param name="selectProcedure"></param>
        public ProductionSimulationSceneInfo(StageType stageType, ProductionMode selectMode, ProcedureType selectProcedure=ProcedureType.Operate)
        {
            StageStyle = StageStyle.Standard;
            SelectStage = stageType;
            SelectMode = selectMode;
            SelectProcedure = selectProcedure;
        }

        public ProductionSimulationSceneInfo(StageType stageType, ProductionMode selectMode, string faultID)
        {
            StageStyle = StageStyle.Fault;
            SelectStage = stageType;
            SelectMode = selectMode;
            FaultID = faultID;
        }
    }


    /// <summary>
    /// 工段主场景中的 数据类
    /// </summary>
    public class StageSceneExamInfo : SceneParam
    {
        /// <summary>
        /// 工段类型
        /// </summary>
        public StageType SelectStage { get; private set; }

        /// <summary>
        /// 选择的模式
        /// </summary>
        public ProductionMode SelectMode { get; private set; }

        /// <summary>
        /// 选择的流程
        /// </summary>
        public ProcedureType SelectProcedure { get; private set; }

        /// <summary>
        /// 故障Id
        /// </summary>
        public string FaultID { get; set; }

        /// <summary>
        /// 工段风格
        /// </summary>
        public StageStyle StageStyle { get; private set; }

        /// <summary>
        /// 考试传递的信息
        /// </summary>
        public ExamTransmitInfo ExamTransmitInfo { get; set; }

        public StageSceneExamInfo()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stageType"></param>
        /// <param name="selectMode"></param>
        /// <param name="selectFlow"></param>
        public StageSceneExamInfo(StageType stageType, ProductionMode selectMode, ProcedureType selectProcedure, ExamTransmitInfo examTransmitInfo)
        {
            StageStyle = StageStyle.Standard;
            SelectStage = stageType;
            SelectMode = selectMode;
            SelectProcedure = selectProcedure;
            ExamTransmitInfo = examTransmitInfo;
        }

        public StageSceneExamInfo(StageType stageType, ProductionMode selectMode, string faultID, ExamTransmitInfo examTransmitInfo)
        {
            StageStyle = StageStyle.Fault;
            SelectStage = stageType;
            SelectMode = selectMode;
            FaultID = faultID;
            ExamTransmitInfo = examTransmitInfo;
        }
    }
}
