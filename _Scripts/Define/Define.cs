using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Simulation;
using XFramework.UI;
using XFramework.Common;
using XFramework.Module;

namespace XFramework
{
    /// <summary>
    /// 界面类型
    /// </summary>
    public enum EnumUIType : int
    {
        /// <summary>
        /// 第三方登录界面
        /// </summary>
        OpenLoginUI,

        /// <summary>
        /// 登录界面
        /// </summary>
        LoginUI,

        /// <summary>
        /// 工艺操作场景
        /// </summary>
        TechnologyOperate,

        /// <summary>
        /// 设备仿真界面
        /// </summary>
        DeviceSimuUI,

        /// <summary>
        /// 漫游主界面
        /// </summary>
        FactoryWalkUI,

        /// <summary>
        /// 过渡加载UI
        /// </summary>
        LoadingUI,

        /// <summary>
        /// 过渡场景UI
        /// </summary>
        LoadingSceneUI,

        /// <summary>
        /// 车间漫游界面
        /// </summary>
        WorkshopRoamingUI,
        /// <summary>
        /// 漫游界面
        /// </summary>
        WalkUI,
        /// <summary>
        /// 生产操作主场景沙盘界面
        /// </summary>
        ProductionMainUI,

        /// <summary>
        /// 生产操作主场景沙盘界面 精简版
        /// </summary>
        ProductionMainLiteUI,
        /// <summary>
        /// 生产线操作仿真界面
        /// </summary>
        ProductionSimulationUI,
        /// <summary>
        /// 工段主场景UI
        /// </summary>
        StageMainUI,
        /// <summary>
        /// 注册界面
        /// </summary>
        RegisterUI,
        /// <summary>
        /// 工程设计界面
        /// </summary>
        EngineeringDesignUI,

        ///// <summary>
        ///// 漫游场景界面
        ///// </summary>
        //WorkshopMainUI,
        /// <summary>
        /// 范例讲解界面
        /// </summary>
        ArchiteIntroduceUI,
        /// <summary>
        /// 固定讲解界面
        /// </summary>
        ArchiteFixedIntroduceUI,
        /// <summary>
        /// 验证仿真UI
        /// </summary>
        ValidationSimulationUI,
        /// <summary>
        /// 验证帮助UI
        /// </summary>
        ValidationHelpUI,
        /// <summary>
        /// 验证主界面
        /// </summary>
        ValidationMainUI,
        
        /// <summary>
        /// 故障仿真界面
        /// </summary>
        FaultSimulationUI,

        /// <summary>
        /// 考试系统UI
        /// </summary>
        ExamSystemUI,

        /// <summary>
        /// 设备拆装界面
        /// </summary>
        EquipmentAssemblyUI,

        /// <summary>
        /// 设备仿真界面
        /// </summary>
        EquipmentSimulationUI,

        /// <summary>
        /// 工艺仿真界面
        /// </summary>
        ProcessSimulationUI,

        /// <summary>
        /// 系統管理界面
        /// </summary>
        SystemManagementUI,
        /// <summary>
        /// 开场视频播放界面
        /// </summary>
        StartVideoPlayUI,
    }

    

    /// <summary>
    /// 工段类型
    /// </summary>
    public enum StageType
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 离心工段
        /// </summary>
        CentrifugeStage,
        /// <summary>
        /// 细胞扩增工段
        /// </summary>
        CellExpansionStage,
        /// <summary>
        /// 细胞培养工段
        /// </summary>
        CellCultivateStage,
        /// <summary>
        /// 培养基配制工段
        /// </summary>
        CultureMediumPreparationStage,
        /// <summary>
        /// 蛋白纯化工段
        /// </summary>
        ProteinPurificationStage,
    }

    /// <summary>
    /// 车间类型
    /// </summary>
    public enum EnumWorkshopType : int
    {
        /// <summary>
        /// 空
        /// </summary>
        None = 0,      
        /// <summary>
        /// 离心车间
        /// </summary>
        CentrifugeWorkshop,
        /// <summary>
        /// 细胞扩增车间
        /// </summary>
        CellExpansionWorkshop,
        /// <summary>
        /// 细胞培养车间
        /// </summary>
        CellCultivateWorkshop,
        /// <summary>
        /// 培养基配制车间
        /// </summary>
        CultureMediumPreparationWorkshop,
       /// <summary>
       /// 蛋白纯化车间
       /// </summary>
        ProteinPurificationWorkshop,
    }

    /// <summary>
    /// 洁净等级枚举
    /// </summary>
    public enum Cleanliness
    {
        AB,
        C,
        D,
        N,
    }

    /// <summary>
    /// 职位
    /// </summary>
    public enum Vocation
    {
        None,
        /// <summary>
        /// 操作工
        /// </summary>
        Operator,
        /// <summary>
        /// 质量保证
        /// </summary>
        QualityAssurance,
        /// <summary>
        /// 质量控制
        /// </summary>
        QualityControl,
        /// <summary>
        /// 物料管理员
        /// </summary>
        MaterialManager,
        /// <summary>
        /// 其他
        /// </summary>
        Others,
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// 男性 
        /// </summary>
        Male,
        /// <summary>
        /// 女性
        /// </summary>
        Female,
    }

    /// <summary>
    /// 故障类型
    /// </summary>
    public enum FaultType
    {
        /// <summary>
        /// 物料泄露
        /// </summary>
        Hydriding_1,
        /// <summary>
        /// 
        /// </summary>
        Hydriding_2,
    }

    /// <summary>
    /// 对话框路径定义
    /// </summary>
    public static class MessageBoxExDefine
    {
        public const string MessageBoxExPath = "Common/MessageBoxEx/";

        /// <summary>
        /// 获取操作对话框的prefab路径
        /// </summary>
        /// <returns></returns>
        public static string GetMessageBoxPath(MessageBoxExEnum type)
        {
            string path = string.Empty;
            switch (type)
            {
                case MessageBoxExEnum.CommonDialog:
                    path = MessageBoxExPath + type.ToString();
                    break;
                case MessageBoxExEnum.SingleDialog:
                    path = MessageBoxExPath + type.ToString();
                    break;
                case MessageBoxExEnum.ProgressDialog:
                    path = MessageBoxExPath + type.ToString();
                    break;
                case MessageBoxExEnum.GameOverDialog:
                    path = MessageBoxExPath + type.ToString();
                    break;
                case MessageBoxExEnum.CheckQuestionDialog:
                    path = MessageBoxExPath + type.ToString();
                    break;
                default:
                    Debug.Log("Not Fipnd MessageBoxExButtons! type: " + type.ToString());
                    break;
            }
            return path;
        }
    }

    /// <summary>
    /// UI定义
    /// </summary>
    public static class UIDefine
    {
        public const string UI_PREFAB = "Assets/_Prefabs/UI/";
        public const string SubUI_PREFAB = "Assets/_Prefabs/SubUI/";

        /// <summary>
        /// 获取UI预设的路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetUIPrefabPath(EnumUIType type)
        {
            string path = string.Empty;
            switch (type)
            {
                case EnumUIType.RegisterUI:
                    path = UI_PREFAB + "RegisterUI";
                    break;
                case EnumUIType.OpenLoginUI:
                    path = UI_PREFAB + "OpenLoginUI";
                    break;
                case EnumUIType.LoginUI:
                    path = UI_PREFAB + "LoginUI";
                    break;
                case EnumUIType.StageMainUI:
                    path = UI_PREFAB + "StageMainUI";
                    break;
                case EnumUIType.ProductionMainUI:
                    path = UI_PREFAB + "ProductionMainUI";
                    break;
                case EnumUIType.ProductionMainLiteUI:
                    path = UI_PREFAB + "ProductionMainLiteUI";
                    break;
                case EnumUIType.ProductionSimulationUI:
                    path = UI_PREFAB + "ProductionSimulationUI";
                    break;
                case EnumUIType.ValidationHelpUI:
                    path = UI_PREFAB + "ValidationHelpUI";
                    break;
                case EnumUIType.FaultSimulationUI:
                    path = UI_PREFAB + "FaultSimulationUI";
                    break;
                case EnumUIType.ExamSystemUI:
                    path = UI_PREFAB + "ExamSystemUI";
                    break;
                case EnumUIType.DeviceSimuUI:
                    path = UI_PREFAB + "DeviceSimuUI";
                    break;
                case EnumUIType.EquipmentAssemblyUI:
                    path = UI_PREFAB + "EquipmentAssemblyUI";
                    break;
                case EnumUIType.EquipmentSimulationUI:
                    path = UI_PREFAB + "EquipmentSimulationUI";
                    break;
                case EnumUIType.EngineeringDesignUI:
                    path = UI_PREFAB + "EngineeringDesignUI";
                    break;
                case EnumUIType.FactoryWalkUI:
                    path = UI_PREFAB + "FactoryWalkUI";
                    break;
                case EnumUIType.WorkshopRoamingUI:
                    path = UI_PREFAB + "WorkshopRoamingUI";
                    break;
                case EnumUIType.LoadingSceneUI:
                    path = UI_PREFAB + "LoadingSceneUI";
                    break;
                case EnumUIType.ProcessSimulationUI:
                    path = UI_PREFAB + "ProcessSimulationUI";
                    break;
                case EnumUIType.SystemManagementUI:
                    path = UI_PREFAB + "SystemManagementUI";
                    break;
                case EnumUIType.StartVideoPlayUI:
                    path = UI_PREFAB + "StartVideoPlayUI";
                    break;
                case EnumUIType.ArchiteIntroduceUI:
                    path = UI_PREFAB + "ArchiteIntroduceUI";
                    break;
                default:
                    Debug.Log("Not Find EnumUIType! type: " + type.ToString());
                    break;
            }
            return path;
        }

        /// <summary>
        /// 获取子UI的路径
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSubUIPrefabPath(string name)
        {
            string path = SubUI_PREFAB + name;
            return path;
        }

        /// <summary>
        /// 获取UI挂载脚本
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetUIScript(EnumUIType type)
        {
            Type scriptType = null;
            switch (type)
            {
                case EnumUIType.RegisterUI:
                    scriptType = typeof(RegisterUI);
                    break;
                case EnumUIType.OpenLoginUI:
                    scriptType = typeof(OpenLoginUI);
                    break;
                case EnumUIType.LoginUI:
                    scriptType = typeof(LoginUI);
                    break;
                case EnumUIType.LoadingSceneUI:
                    scriptType = typeof(LoadingSceneUI);
                    break;
                case EnumUIType.ProductionMainUI:
                    scriptType = typeof(ProductionMainUI);
                    break;
                case EnumUIType.ProductionMainLiteUI:
                    scriptType = typeof(ProductionMainLiteUI);
                    break;
                case EnumUIType.ProductionSimulationUI:
                    scriptType = typeof(ProductionSimulationUI);
                    break;
                case EnumUIType.ValidationHelpUI:
                    scriptType = typeof(ValidationHelpUI);
                    break;
                case EnumUIType.FaultSimulationUI:
                    scriptType = typeof(FaultSimulationUI);
                    break;
                case EnumUIType.ExamSystemUI:
                    scriptType = typeof(ExamSystemUI);
                    break;
                case EnumUIType.EquipmentAssemblyUI:
                    scriptType = typeof(EquipmentAssemblyUI);
                    break;
                case EnumUIType.EquipmentSimulationUI:
                    scriptType = typeof(EquipmentSimulationUI);
                    break;
                case EnumUIType.EngineeringDesignUI:
                    scriptType = typeof(EngineeringDesignUI);
                    break;
                case EnumUIType.FactoryWalkUI:
                    scriptType = typeof(FactoryWalkUI);
                    break;
                case EnumUIType.WorkshopRoamingUI:
                    scriptType = typeof(WorkshopRoamingUI);
                    break;
                case EnumUIType.ProcessSimulationUI:
                    scriptType = typeof(ProcessSimulationUI);
                    break;
                case EnumUIType.SystemManagementUI:
                    scriptType = typeof(SystemManagementUI);
                    break;
                case EnumUIType.StartVideoPlayUI:
                    scriptType = typeof(StartVideoPlayUI);
                    break;
                case EnumUIType.ArchiteIntroduceUI:
                    scriptType = typeof(ArchiteIntroduceUI);
                    break;
                default:
                    Debug.Log("Not Find EnumUIType! type: " + type.ToString());
                    break;
            }
            return scriptType;
        }


    }

    /// <summary>
    /// 场景定义类型
    /// </summary>
    public static class SceneDefine
    {
        /// <summary>
        /// 获取场景名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSceneName(SceneType type)
        {
            string name = string.Empty;
            switch (type)
            {
                case SceneType.LoginScene:
                    name = "LoginScene";
                    break;
                case SceneType.LoadingScene:
                    name = "LoadingScene";
                    break;
                case SceneType.WorkShopMainScene:
                    name = "WorkShopMainScene";
                    break;
                case SceneType.WalkScene:
                    name = "WalkScene";
                    break;
                case SceneType.StageScene:
                    name = "StageScene";
                    break;
                case SceneType.StageMainScene:
                    name = "StageMainScene";
                    break;
                case SceneType.EquipmentSimulationScene:
                    name = "DeviceSimuScene";
                    break;
                case SceneType.EngineeringDesignScene:
                    name = "EngineeringDesignScene";
                    break;
                case SceneType.FactoryWalkScene:
                    name = "FactoryWalkScene";
                    break;
                case SceneType.WorkshopRoamingScene:
                    name = "WorkshopRoamingScene";
                    break;
                case SceneType.ProcessSimulationScene:
                    name = "ProcessSimulationScene";
                    break;
                case SceneType.ProductionMainScene:
                    name = "ProductionMainScene";
                    break;
                case SceneType.ProductionSimulationScene:
                    name = "ProductionSimulationScene";
                    break;
                case SceneType.ValidationSimulationScene:
                    name = "ValidationSimulationScene";
                    break;
                case SceneType.PureWaterScene:
                    name = "PureWaterScene";
                    break;
                case SceneType.ValidationMainScene:
                    name = "ValidationMainScene";
                    break;
                case SceneType.ExamSystemScene:
                    name = "ExamSystemScene";
                    break;
                case SceneType.CellExpansionStageScene:
                    name = "CellExpansionStageScene";
                    break;

                default:
                    break;
            }
            return name;
        }
    }

    /// <summary>
    /// 车间定义
    /// </summary>
    public static class WorkshopDefine
    {
        public const string Workshop_PREFAB = "Assets/_Prefabs/Workshop/";

        /// <summary>
        /// 获取车间的prefab路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetWorkshopPrefabPath(EnumWorkshopType type)
        {
            string path = string.Empty;

            switch (type)
            {
                case EnumWorkshopType.None:
                    break;              
                case EnumWorkshopType.CentrifugeWorkshop:
                    path = Workshop_PREFAB + "离心间";
                    break;
                case EnumWorkshopType.CellExpansionWorkshop:
                    path = Workshop_PREFAB + "细胞扩增间";
                    break;
                case EnumWorkshopType.CellCultivateWorkshop:
                    path = Workshop_PREFAB + "细胞培养间";
                    break;
                case EnumWorkshopType.CultureMediumPreparationWorkshop:
                    path = Workshop_PREFAB + "培养基配制间";
                    break;
                case EnumWorkshopType.ProteinPurificationWorkshop:
                    path = Workshop_PREFAB + "蛋白纯化间";
                    break;
                default:
                    Debug.Log("Not Find EnumWorkshopType! type: " + type.ToString());
                    break;
            }

            return path;
        }

        /// <summary>
        /// 获取车间的Stcript
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetWorkshopScript(EnumWorkshopType type)
        {
            Type scriptType = null;

            switch (type)
            {
                case EnumWorkshopType.None:
                    break;             
                case EnumWorkshopType.CentrifugeWorkshop:
                    scriptType = typeof(CentrifugeWorkshop);
                    break;
                case EnumWorkshopType.CellExpansionWorkshop:
                    scriptType = typeof(CellExpansionWokeshop);
                    break;
                case EnumWorkshopType.CellCultivateWorkshop:
                    scriptType = typeof(CellCultivateWorkshop);
                    break;
                case EnumWorkshopType.CultureMediumPreparationWorkshop:
                    scriptType = typeof(CultureMediumPreparationWorkshop);
                    break;
                case EnumWorkshopType.ProteinPurificationWorkshop:
                    scriptType = typeof(ProteinPurificationWorkshop);
                    break;
                default:
                    Debug.Log("Not Find EnumWorkshopType! type: " + type.ToString());
                    break;
            }

            return scriptType;
        }

        /// <summary>
        /// 获取车间名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetWorkshopName(EnumWorkshopType type)
        {
            string workshop_name = string.Empty;

            switch (type)
            {
                case EnumWorkshopType.None:
                    break;
                case EnumWorkshopType.CentrifugeWorkshop:
                    workshop_name = "离心间";
                    break;
                case EnumWorkshopType.CellExpansionWorkshop:
                    workshop_name = "细胞扩增间";
                    break;
                case EnumWorkshopType.CellCultivateWorkshop:
                    workshop_name = "细胞培养间";
                    break;
                case EnumWorkshopType.CultureMediumPreparationWorkshop:
                    workshop_name = "培养基配制间";
                    break;
                case EnumWorkshopType.ProteinPurificationWorkshop:
                    workshop_name = "蛋白纯化间";
                    break;
                default:
                    break;
            }

            return workshop_name;
        }
    }

    /// <summary>
    /// 工段定义
    /// </summary>
    public static class StageDefine
    {
        public static Type GetStageScript(StageType stageType)
        {
            Type t = null;

            switch (stageType)
            {
                case StageType.CentrifugeStage:
                    t = typeof(CentrifugeStage);
                    break;
                case StageType.CellExpansionStage:
                    t = typeof(CellExpansionStage);
                    break;
                case StageType.CellCultivateStage:
                    t = typeof(CellCultivateStage);
                    break;
                case StageType.CultureMediumPreparationStage:
                    t = typeof(CultureMediumPreparationStage);
                    break;
                case StageType.ProteinPurificationStage:
                    t = typeof(ProteinPurificationStage);
                    break;
                default:
                    break;
            }

            return t;
        }

        /// <summary>
        /// 获取工段名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetStageName(StageType type)
        {
            string name = null;
            switch (type)
            {
                case StageType.CentrifugeStage:
                    name = "离心工段";
                    break;
                case StageType.CellExpansionStage:
                    name = "细胞扩增工段";
                    break;
                case StageType.CellCultivateStage:
                    name = "细胞培养工段";
                    break;
                case StageType.CultureMediumPreparationStage:
                    name = "培养基配制工段";
                    break;
                case StageType.ProteinPurificationStage:
                    name = "蛋白纯化工段";
                    break;
                default:
                    break;
            }
            return name;

        }

        /// <summary>
        /// 获取工段视频路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetStageVideoPath(StageType type)
        {
            string path = string.Empty;
            switch (type)
            {
                default:
                    break;
            }
            return path;
        }
    }

    /// <summary>
    /// 角色定义
    /// </summary>
    public static class CharacterDefine
    {
        private const string PrefabPath = "Assets/_Prefabs/Characters/";

        /// <summary>
        /// 获取Character路径
        /// </summary>
        /// <param name="cleanliness"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static string GetCharacterPrefabPath(Cleanliness cleanliness, Gender gender)
        {
            string path = string.Empty;
            string sex = string.Empty;

            switch (gender)
            {
                case Gender.Male:
                    sex = "男";
                    break;
                case Gender.Female:
                    sex = "女";
                    break;
                default:
                    break;
            }

            switch (cleanliness)
            {
                case Cleanliness.AB:
                    path = PrefabPath + "A&B_" + sex;
                    break;
                case Cleanliness.C:
                    path = PrefabPath + "C_" + sex;
                    break;
                case Cleanliness.D:
                    path = PrefabPath + "D_" + sex;
                    break;
                case Cleanliness.N:
                    path = PrefabPath + "N_" + sex;
                    break;
                default:
                    Debug.Log("Not Find Person! type: " + cleanliness.ToString() + "--" + sex);
                    break;
            }
            return path;
        }
    }

    /// <summary>
    /// 验证类型
    /// </summary>
    public enum ValidationType
    {
        None,
    }

    /// <summary>
    /// 验证定义
    /// </summary>
    public static class ValidationDefine
    {
        public static Type GetValidationScript(ValidationType validationType)
        {
            Type t = null;

            switch (validationType)
            {
                case ValidationType.None:
                    break;
                default:
                    break;
            }

            return t;
        }

        /// <summary>
        /// 获取验证名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetValidationName(ValidationType type)
        {
            string name = null;
            switch (type)
            {
                case ValidationType.None:
                    break;
                default:
                    break;
            }
            return name;
        }
    }

    public static class ValveDefine
    {
        public static ValveType GetValveType(string name)
        {
            ValveType type = ValveType.None;
            switch (name)
            {
                case "手动球阀":
                    type = ValveType.VQ;
                    break;
                case "手动截止阀":
                    type = ValveType.VJ;
                    break;
                case "安全阀":
                    type = ValveType.VA;
                    break;
                case "止回阀":
                    type = ValveType.VH;
                    break;
                case "气动截止阀":
                    type = ValveType.SV;
                    break;
                case "气动球阀":
                    type = ValveType.SQ;
                    break;
                case "法兰电磁阀":
                    type = ValveType.DC;
                    break;
                case "气动隔膜阀":
                    type = ValveType.SG;
                    break;
                default:
                    break;
            }
            return type;
        }

        public static string GetValveName(ValveType type)
        {
            string name = string.Empty;
            switch (type)
            {
                case ValveType.VQ:
                    name = "手动球阀";
                    break;
                case ValveType.VJ:
                    name = "手动截止阀";
                    break;
                case ValveType.VA:
                    name = "安全阀";
                    break;
                case ValveType.VH:
                    name = "止回阀";
                    break;
                case ValveType.SV:
                    name = "气动截止阀";
                    break;
                case ValveType.SQ:
                    name = "气动球阀";
                    break;
                case ValveType.DC:
                    name = "法兰电磁阀";
                    break;
                case ValveType.SG:
                    name = "气动隔膜阀";
                    break;
                default:
                    break;
            }
            return name;
        }
    }
}
