using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 总体介绍窗口ui参数
    /// </summary>
    public class OverallUiArgs : BaseUIArgs
    {
        /// <summary>
        /// 介绍信息
        /// </summary>
        public IntroduceContent introduceInfo { get; set; }


        /// <summary>
        /// 是否显示“进入”按钮
        /// </summary>
        public bool IsShowEnterBtn { get; set; }

        /// <summary>
        /// 进入房间事件
        /// </summary>
        public Action EnterAction { get; set; }

        /// <summary>
        /// 退出房间事件
        /// </summary>
        public Action ExitAction { get; set; }

        /// <summary>
        /// 关闭窗口事件
        /// </summary>
        public Action CloseAction { get; set; }

    
    }

}
