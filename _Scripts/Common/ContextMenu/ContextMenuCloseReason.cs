using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.UI
{
    public enum ContextMenuHideReason
    {
        /// <summary>
        /// 控件关闭的原因是另一应用程序接收了焦点。
        /// </summary>
        AppFocusChange = 0,
        /// <summary>
        /// 控件关闭的原因是启动了某个应用程序。
        /// </summary>
        AppClicked = 1,
        /// <summary>
        /// 控件关闭的原因是单击了该控件的其中一项。
        /// </summary>
        ItemClicked = 2,
        /// <summary>
        /// 控件关闭的原因是由于键盘活动，如按下了 Esc 键。
        /// </summary>
        Keyboard = 3,
        /// <summary>
        /// 控件关闭的原因是调用了 System.Windows.Forms.ToolStripDropDown.Close()
        /// </summary>
        CloseCalled = 4,
    }
}
