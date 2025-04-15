using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
using DG.Tweening;
  
 namespace XFramework.Common
 {  /// <summary>
    /// 标签 组件
    /// </summary>
     public class HUDLabelComponent : HUDComponent<HUDLabelInfo>
    {

    
        public override void show()
        {
            if (HasCreated)
            {
                hudInfo.onScreenArgs.visible = true;

            }
            else
            {
                CreateHUD();
            }
        }
        public override void hide()
        {

            if (HasCreated)
            {
                hudInfo.onScreenArgs.visible = false;
            }
        }
        /// <summary>
        /// 设置标签内容
        /// </summary>
        /// <param name="info"></param>
        public void SetHudLableContentInfo(string  info)
        {
            hudInfo.onScreenArgs.m_Content = info;
        }

        public void SetLableText(string text)
        {
            hudInfo.onScreenArgs.m_Text = text;
        }
       
     }
 }
