using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Module;
using XFramework.UI;
using System;

namespace XFramework.Simulation.Component
{
    /// <summary>
    /// 漫游进入房间参数
    /// </summary>
	public class EnterWorkshopArgs  {
        public string  Name { get; set; }


        public bool isShowEnterButton { get; set; }
        /// <summary>
        /// 进入
        /// </summary>
        public Action EnterAction { get; set; }

        /// <summary>
        /// 退出
        /// </summary>
        public Action ExitAction { get; set; }

       

    }
}