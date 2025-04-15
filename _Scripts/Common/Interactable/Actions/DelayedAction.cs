using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public class DelayedAction : ActionBase
    {
        /// <summary>
        /// 延迟时间
        /// </summary>
        public float delay;

        public DelayedAction(float _delay)
        {
            delay = _delay;
        }

        public override void Execute()
        {
            CoroutineManager.Instance.StartCoroutine(ActionCoroutine(delay));
        }

        protected IEnumerator ActionCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            Completed();
        }
    }
}
