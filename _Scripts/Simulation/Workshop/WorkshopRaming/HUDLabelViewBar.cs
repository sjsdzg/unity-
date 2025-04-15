using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
namespace XFramework.UI
{
    /// <summary>
    /// HUD label视图窗口
    /// </summary>
	public class HUDLabelViewBar : BaseRamingUI
    {
		// Use this for initialization
		void Start () {
            ShowMode = UIWindowShowMode.HideOther;
        }

        // Update is called once per frame
        void Update () {
			
		}
        public override void Show(BaseUIArgs args)
        {
            base.Show(args);
            print("打开hud窗口");

        }
        public override void Hide(BaseUIArgs args)
        {
            base.Hide(args);
            Messager.Instance.SendMessage(new Message(MessageType.WorkshopGmpMsg, this, new RamingUI2GmpMsg(false, null)));
            print("关闭hud窗口");
        }
    }
}