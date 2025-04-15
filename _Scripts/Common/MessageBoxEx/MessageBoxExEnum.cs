using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    #region MessageBoxExType 枚举
    /// <summary>
    /// 操作对话框类型 
    /// </summary>
    public enum MessageBoxExEnum
    {
        /// <summary>
        /// 通用对话框
        /// </summary>
        CommonDialog,
        /// <summary>
        /// 单个按钮OK对话框
        /// </summary>
        SingleDialog,
        /// <summary>
        /// 进度条对话框
        /// </summary>
        ProgressDialog,
        /// <summary>
        /// 游戏退出对话框
        /// </summary>
        GameOverDialog,
        /// <summary>
        /// 问答题
        /// </summary>
        CheckQuestionDialog,
    }
    #endregion
}
