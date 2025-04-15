using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Simulation;

namespace XFramework.Component
{
    /// <summary>
    /// 气动阀门组件
    /// </summary>
    public class GasValveComponent : ComponentBase
    {
        void Start()
        {
            //CatchToolTip = name.Split('-')[0];
        }

        private int isOpened;
        /// <summary>
        /// 是否打开
        /// </summary>
        public int IsOpened
        {
            get { return isOpened; }
            set { isOpened = value; }
        }
    }
}
