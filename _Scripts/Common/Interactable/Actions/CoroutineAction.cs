using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    public class CoroutineAction : ActionBase
    {
        /// <summary>
        /// IEnumerator
        /// </summary>
        IEnumerator routine;

        public CoroutineAction(IEnumerator _routine)
        {
            routine = _routine;
        }

        public override void Execute()
        {
            if (routine != null)
            {
                CoroutineManager.Instance.StartCoroutine(CallWrapper());
            }
            else
            {
                Error(new Exception("routine is null"));
            }
        }

        IEnumerator CallWrapper()
        {
            while (routine.MoveNext())
            {
                yield return routine.Current;
            }
            Completed();
        }
    }
}
