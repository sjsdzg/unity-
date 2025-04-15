using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 右侧多个窗口管理
    /// </summary>
	public class RightUIGroup : MonoBehaviour{


  //      private OverallInfoBar m_OverallInfoBar;

  //      private MiniDeviceInfoBar m_DeviceInfoBar;
  //      //public List<IDisplay> list = new List<IDisplay>();
  //      //public Action overallClose;

  //      //public Action DeviceClose;
		//// Use this for initialization
		//void Start () {
  //          m_DeviceInfoBar = transform.Find("MiniDeviceInfoBar").GetComponent<MiniDeviceInfoBar>();
  //          m_OverallInfoBar = transform.Find("OverallInfoBar").GetComponent<OverallInfoBar>();

  //          m_DeviceInfoBar.IsActive = false;
  //          m_OverallInfoBar.IsActive = false;

  //      }

  //      /// <summary>
  //      /// 总体介绍窗口显示
  //      /// </summary>
  //      /// <param name="Info"></param>
  //      /// <param name="isBtnShow"></param>
  //      /// <param name="m_EnterAction"></param>
  //      /// <param name="m_CloseAction"></param>
  //      //public void ShowOverallWindow(IntroduceContent Info,bool isBtnShow,Action m_EnterAction,Action m_CloseAction)
  //      //{

  //      //    m_OverallInfoBar.OverallContentShow(Info, isBtnShow,m_EnterAction,()=> {
  //      //        m_CloseAction.Invoke();
  //      //        HideRightWindow();
  //      //    });
  //      //    m_DeviceInfoBar.HideWindow();
  //      //}
  //      /// <summary>
  //      /// 设备显示
  //      /// </summary>
  //      /// <param name="Info"></param>
  //      /// <param name="m_Action"></param>
  //      public void ShowDeviceWindow(MachineItem Info, Action _CloseAction)
  //      {
  //          m_DeviceInfoBar.ExhibitionEquipShow(Info, ()=> {
  //              _CloseAction.Invoke();
  //              HideRightWindow();
  //          });
  //          m_OverallInfoBar.HideWindow();
  //      }
  //      public void HideRightWindow()
  //      {
  //          m_DeviceInfoBar.HideWindow();
  //          m_OverallInfoBar.HideWindow();

  //      }
    }
}