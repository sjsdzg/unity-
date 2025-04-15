using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class DOLocalRotaAction : ActionBase
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

        public RotateMode rotateMode;

        private LoopType m_loopType;

        private bool m_IsLoop;

        private int m_LoopTime;
        private bool isCompleted;

        public DOLocalRotaAction(Transform _transform, Vector3 _endValue, float _duration,RotateMode _rMode= RotateMode.Fast)
        {
            transform = _transform;
            endValue = _endValue;
            duration = _duration;
            rotateMode = _rMode;
        }
        public DOLocalRotaAction(Transform _transform, Vector3 _endValue, float _duration,bool _isCompleted, RotateMode _rMode = RotateMode.Fast)
        {
            transform = _transform;
            endValue = _endValue;
            duration = _duration;
            rotateMode = _rMode;
            isCompleted = _isCompleted;
        }
        public DOLocalRotaAction(Transform _transform, Vector3 _endValue, float _duration, bool isLoop,int loopTime,LoopType loopType= LoopType.Incremental, RotateMode _rMode = RotateMode.Fast)
        {
            transform = _transform;
            endValue = _endValue;
            duration = _duration;
            rotateMode = _rMode;
            m_IsLoop = isLoop;
            m_LoopTime = loopTime;
            m_loopType = loopType;
        }


        public override void Execute()
        {
            if (!isCompleted)
            {
                transform.DOLocalRotate(endValue, duration, rotateMode).OnComplete(() => {
                    Completed();
                    MonoBehaviour.print("旋转：" + transform.name + "  ");
                });
            }
            else
            {
                Completed();
                transform.DOLocalRotate(endValue, duration, rotateMode);
            }

            if (m_IsLoop)
            {
                transform.DOLocalRotate(endValue, duration, rotateMode).SetLoops(m_LoopTime, m_loopType).OnComplete(() => {
                    Completed();
                    
                });
            }
        }
    }
}
