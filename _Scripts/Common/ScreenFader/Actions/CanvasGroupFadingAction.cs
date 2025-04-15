using UnityEngine;
using System.Collections;
using XFramework.Common;
using System;
using DG.Tweening;

namespace XFramework.Common
{
    public class CanvasGroupFadingAction : ActionBase
    {
        /// <summary>
        /// CanvasGroup
        /// </summary>
        public CanvasGroup canvasGroup;

        /// <summary>
        /// 结束
        /// </summary>
        public float endValue = 1;

        /// <summary>
        /// Duration
        /// </summary>
        public float duration = 1;

        public CanvasGroupFadingAction(CanvasGroup _canvasGroup, float _endValue = 1, float _duration = 1)
        {
            canvasGroup = _canvasGroup;
            duration = _duration;
            endValue = _endValue;
        }

        public override void Execute()
        {
            canvasGroup.DOFade(endValue, duration).OnComplete(() => {
                //完成
                Completed();
            });
        }
    }
}

