using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Architectural
{
    /// <summary>
    /// 视图模式
    /// </summary>
    public enum ViewMode
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 图纸模式
        /// </summary>
        Drawing,
        /// <summary>
        /// 立面模式
        /// </summary>
        Facade,
    }
    
    /// <summary>
    /// 轴模式
    /// </summary>
    [Flags]
    public enum AxisMode
    {
        N = 0,
        X = 1,
        Y = 2,
        Z = 4,
    }

    /// <summary>
    /// 选择状态
    /// </summary>
    public enum SelectionState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 高亮
        /// </summary>
        Highlighted,
        /// <summary>
        /// 选中
        /// </summary>
        Selected,
    }
}
