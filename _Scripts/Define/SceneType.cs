using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    /// <summary>
    /// 场景类型
    /// </summary>
    public enum SceneType : int
    {
        /// <summary>
        /// 空场景
        /// </summary>
        None = 0,
        /// <summary>
        /// 启动场景
        /// </summary>
        BootstrapScene,
        /// <summary>
        /// 登录场景
        /// </summary>
        LoginScene,
        /// <summary>
        /// 过渡场景
        /// </summary>
        LoadingScene,
        /// <summary>
        /// 漫游主场景
        /// </summary>
        WorkShopMainScene,
        /// <summary>
        /// 漫游场景
        /// </summary>
        WalkScene,
        /// <summary>
        /// 灌装冻干场景
        /// </summary>
        StageScene,
        /// <summary>
        /// 工段场景选择主场景
        /// </summary>
        StageMainScene,
        /// <summary>
        /// 设备仿真场景
        /// </summary>
        EquipmentSimulationScene,
        /// <summary>
        /// 工程设计场景
        /// </summary>
        EngineeringDesignScene,
        /// <summary>
        /// 厂区漫游场景
        /// </summary>
        FactoryWalkScene,
        /// <summary>
        /// 车间漫游场景
        /// </summary>
        WorkshopRoamingScene,
        /// <summary>
        /// 工艺仿真场景
        /// </summary>
        ProcessSimulationScene,
        /// <summary>
        /// 生产操作仿真主场景
        /// </summary>
        ProductionMainScene,
        /// <summary>
        /// 生产操作仿真Lite主场景
        /// </summary>
        ProductionMainLiteScene,
        /// <summary>
        /// 生产操作仿真场景
        /// </summary>
        ProductionSimulationScene,
        /// <summary>
        /// 验证操作场景
        /// </summary>
        ValidationSimulationScene,
        /// <summary>
        /// 制水操作场景
        /// </summary>
        PureWaterScene,
        /// <summary>
        /// 验证主场景
        /// </summary>
        ValidationMainScene,
        /// <summary>
        /// 考试系统场景
        /// </summary>
        ExamSystemScene,
        /// <summary>
        /// 系统管理场景
        /// </summary>
        SystemManagementScene,
        /// <summary>
        /// 陕西中医药大学补充场景
        /// </summary>
        VirtualSectionScene,
        /// <summary>
        /// 细胞扩增场景
        /// </summary>
        CellExpansionStageScene,
        /// <summary>
        /// 细胞培养场景
        /// </summary>
        CellCultivateStageScene,
        /// <summary>
        /// 蛋白纯化场景
        /// </summary>
        ProteinPurificationStageScene,
        /// <summary>
        /// 培养基配制场景
        /// </summary>
        CultureMediumPreparationScene,
        /// <summary>
        /// 开场播放视频场景
        /// </summary>
        StartVideoPlayScene,
        /// <summary>
        /// 工艺流程设计场景
        /// </summary>
        ProcessDesignSampleScene,
        /// <summary>
        /// 车间设计场景
        /// </summary>
        ArchitectScene,
        /// <summary>
        /// 范例讲解场景
        /// </summary>
        ArchiteIntroduceScene,
        /// <summary>
        /// 固定条件讲解场景
        /// </summary>
        ArchiteFixedIntroduceScene,
    }
}
