using UnityEngine;
using System.Collections;
using System;

namespace XFramework.Common
{
    public class CanvasGroupBlocksAction : ActionBase
    {
        /// <summary>
        /// CanvasGroup
        /// </summary>
        public CanvasGroup canvasGroup;

        /// <summary>
        /// State
        /// </summary>
        public bool state;

        public CanvasGroupBlocksAction(CanvasGroup _canvasGroup, bool _state)
        {
            canvasGroup = _canvasGroup;
            state = _state;
        }

        public override void Execute()
        {
            canvasGroup.blocksRaycasts = state;
            Completed();
        }
    }
}

