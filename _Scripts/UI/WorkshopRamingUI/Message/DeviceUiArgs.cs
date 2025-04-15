using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Module;

namespace XFramework.UI

{


    /// <summary>
    /// 设备窗口的参数
    /// </summary>
    public class DeviceUiArgs :BaseUIArgs
    {
        /// <summary>
        /// 设备信息
        /// </summary>
        public MachineItem machineInfo { get; set; }

        /// <summary>
        /// 关闭事件
        /// </summary>
        public Action CloseAction { get; set; }

    }
}
