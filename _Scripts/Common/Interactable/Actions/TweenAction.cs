using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;

namespace XFramework.Common
{
    /// <summary>
    /// 支持DOTween
    /// </summary>
    public class TweenAction : ActionBase
    {
        /// <summary>
        /// Tweener
        /// </summary>
        private Tweener tweener;

        public TweenAction(Tweener _tween)
        {
            tweener = _tween;
            tweener.Pause();
        }

        public override void Execute()
        {
            if (tweener != null)
            {
                tweener.OnComplete(() => Completed());
                tweener.Play();
            }
            else
            {
                Error(new Exception("tween is null"));
            }
        }
    }
}
