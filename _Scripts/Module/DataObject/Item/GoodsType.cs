using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 物品项类型
    /// </summary>
    public enum GoodsType
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        #region 离心工段
        /// <summary>
        /// 离心间滤布
        /// </summary>
        LXJ_LB_01,
        /// <summary>
        /// 离心间物料软管
        /// </summary>
        LXJ_RG_01,
        /// <summary>
        /// 离心间纯化水软管
        /// </summary>
        LXJ_RG_02,
        /// <summary>
        /// 离心间氮气软管
        /// </summary>
        LXJ_RG_03,
        /// <summary>
        /// 离心间物料桶
        /// </summary>
        LXJ_WUT_01,
        /// <summary>
        /// 离心间滤袋滤饼
        /// </summary>
        LXJ_LDLB_01,
        #endregion
        #region 细胞扩增工段
        /// <summary>
        /// 培养基瓶
        /// </summary>
        Cell_PYJP_01,
        /// <summary>
        /// 实验器材
        /// </summary>
        Cell_SYQC_01,
        /// <summary>
        /// 冻存盒
        /// </summary>
        Cell_DCH_01,
        /// <summary>
        /// 冻存管
        /// </summary>
        Cell_DCG_01,
        /// <summary>
        /// 离心管
        /// </summary>
        Cell_LXG_01,
        /// <summary>
        /// 小摇瓶
        /// </summary>
        Cell_XYP_01,
        /// <summary>
        /// 大摇瓶
        /// </summary>
        Cell_DYP_01,
        #endregion
        #region 细胞培养工段
        /// <summary>
        /// 培养罐
        /// </summary>
        Cell_PYG_01,
        /// <summary>
        /// 细胞培养瓶
        /// </summary>
        Cell_PYP_01,
        /// <summary>
        /// 注射器
        /// </summary>
        Cell_ZSQ_01,
        /// <summary>
        /// 火焰线圈
        /// </summary>
        Cell_HYXQ_01,
        /// <summary>
        /// 取样瓶
        /// </summary>
        Cell_QYP_01,
        /// <summary>
        /// 取样瓶(有液体)
        /// </summary>
        Cell_QYP_02,
        #endregion
        #region 培养基配制工段
        /// <summary>
        /// 烧杯
        /// </summary>
        PYG_SB_01,
        /// <summary>
        /// 抹布
        /// </summary>
        PYG_MB_01,
        /// <summary>
        /// 物料桶
        /// </summary>
        PYG_WLT_01,
        /// <summary>
        /// F001S
        /// </summary>
        PYG_F001S_01,
        /// <summary>
        /// 除菌过滤器
        /// </summary>
        PYG_FILTER_01,
        /// <summary>
        /// 补料瓶（取样前）
        /// </summary>
        PYG_BLP_01,
        /// <summary>
        /// 补料瓶（取样后）
        /// </summary>
        PYG_BLP_02,

        /// <summary>
        /// F001S
        /// </summary>
        PYG_F001B_01,

        PYG_200LMAT_01,

        PYG_200LMAT_02,

        PYG_200LMAT_03,

        PYG_M20A_01,
        #endregion
        #region 蛋白纯化工段
        /// <summary>
        /// 样品收集桶
        /// </summary>
        DBCH_YPSJT_01,
        /// <summary>
        /// 量筒
        /// </summary>
        DBCH_LT_01,
        /// <summary>
        ///软管
        /// </summary>
        DBCH_RG_01,
        /// <summary>
        ///硅胶管
        /// </summary>
        DBCH_GJG_01,
        /// <summary>
        ///洗脱液桶
        /// </summary>
        DBCH_XTYT_01,
        /// <summary>
        ///纳滤膜包
        /// </summary>
        DBCH_NLMB_01,
        /// <summary>
        ///原液桶
        /// </summary>
        DBCH_YYT_01,

        #endregion
    }
}
