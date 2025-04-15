using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.UI
{
    /// <summary>
    /// 上下文菜单栏显示方位
    /// </summary>
    public enum ContextMenuDirection
    {
        /// <summary>
        /// 显示在其控件的上方并位于其左侧。
        /// </summary>
        AboveLeft = 0,
        /// <summary>
        /// 显示在其控件的上方并位于其右侧。
        /// </summary>
        AboveRight = 1,
        /// <summary>
        /// 显示在其控件的下方并位于其左侧。
        /// </summary>
        BelowLeft = 2,
        /// <summary>
        /// 显示在父控件的下方并位于其右侧。
        /// </summary>
        BelowRight = 3,
    }
}
