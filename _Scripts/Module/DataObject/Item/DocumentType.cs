using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 文件项类型
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 考核报告
        /// </summary>
        AssessmentReport,
        /// <summary>
        /// 领料文件
        /// </summary>
        PickingDocument,
        /// <summary>
        /// 结晶干燥-缬沙坦样品检验
        /// </summary>
        JJGZ_XSTYPJY_01,
        /// <summary>
        /// 外包间-物料桶标签
        /// </summary>
        WBJ_WLTBQ_01,
    }
}
