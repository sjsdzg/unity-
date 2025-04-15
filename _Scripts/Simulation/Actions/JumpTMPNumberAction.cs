using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.Actions
{
    public class JumpTMPNumberAction : ActionBase
    {
        private int end;
        private int start;
        private int jumpTimes = 30;
        private int result=0;
        private TextMeshPro tmp;



        public JumpTMPNumberAction(TextMeshPro _tmp, int _start,int _end, int _jumpTimes)
        {
            tmp = _tmp;
            start = _start;
            jumpTimes = _jumpTimes;
            end = _end;
        }
        public override void Execute()
        {
 
            CoroutineManager.Instance.StartCoroutine(JumpNumber());
        
        }
        public IEnumerator JumpNumber()
        {
            int delta = (end - start) / jumpTimes;
            for (int i = 0; i < jumpTimes; i++)
            {
                result += delta;

                tmp.text = result.ToString();
                yield return new WaitForSeconds(0.1f);
            }

            result = end;
            tmp.text = result.ToString();
            CoroutineManager.Instance.StopAllCoroutines();
            Completed();
        }
    }
}
