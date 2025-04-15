using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;
using XFramework.Common;
using UnityEngine.Events;

namespace XFramework.Simulation
{
    public class ScreenFaderAction : ActionBase
    {
        /// <summary>
        /// 显示的内容
        /// </summary>
        private string textContent;
        /// <summary>
        /// 等待时间
        /// </summary>
        private int pause;
        /// <summary>
        /// 字体透明度
        /// </summary>
        private int textAlpha;

        public ScreenFaderAction(string _textContent, int _pause, int _textAlpha)
        {
            textContent = _textContent;
            pause = _pause;
            textAlpha = _textAlpha;
        }
        public override void Execute()
        {

            ScreenFader.Instance
                .OnRemoveAction()
                .FadeIn(0)
                .SetText(textContent, textAlpha)
                .FadeTextIn()
                .Pause(pause)
                .FadeTextOut()
                .FadeOut()
                .OnCompleted(() => { Completed(); })
                .Execute();

        }
        public override void Completed()
        {
            base.Completed();
        }
    }
}
