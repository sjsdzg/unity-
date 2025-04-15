using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class DOLocalMoveAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public Transform transform;

        /// <summary>
        /// endValue
        /// </summary>
        public Vector3 endValue;

        /// <summary>
        /// duration
        /// </summary>
        public float duration;
        private bool isCompleted;

        public DOLocalMoveAction(Transform _transform, Vector3 _endValue, float _duration, bool _isCompleted=false)
        {
            transform = _transform;
            endValue = _endValue;
            duration = _duration;
            isCompleted = _isCompleted;
        }

        public override void Execute()
        {
            if (isCompleted)
            {
                Completed();
                transform.DOLocalMove(endValue, duration);
            }
            else
            {
                transform.DOLocalMove(endValue, duration).OnComplete(() => { Completed(); });
            }                    
        }
    }
}
