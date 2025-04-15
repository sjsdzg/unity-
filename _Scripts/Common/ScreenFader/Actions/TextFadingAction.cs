using UnityEngine;
using System.Collections;
using XFramework.Common;
using System;
using DG.Tweening;
using UnityEngine.UI;

namespace XFramework.Common
{
    public class TextFadingAction : ActionBase
    {
        /// <summary>
        /// CanvasGroup
        /// </summary>
        public Text text;

        /// <summary>
        /// 结束
        /// </summary>
        public float endValue = 1;

        /// <summary>
        /// Duration
        /// </summary>
        public float duration = 1;

        public TextFadingAction(Text _text, float _endValue = 1, float _duration = 1)
        {
            text = _text;
            endValue = _endValue;
            duration = _duration;
        }

        public override void Execute()
        {
            text.DOFade(endValue, duration).OnComplete(() => {
                //完成
                Completed();
            });
        }
    }
}

