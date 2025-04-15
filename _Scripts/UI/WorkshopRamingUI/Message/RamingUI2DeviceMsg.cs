using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Simulation.Component
{
    /// <summary>
    /// 界面发送设备相关的消息
    /// </summary>
	public class RamingUI2DeviceMsg{

        public bool IsShowDevice;

        //public bool 
        public RamingUI2DeviceMsg(bool isShow)
        {

            IsShowDevice = isShow;
        }
	}
}