using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;
using XFramework.Simulation;
using XFramework.Core;

namespace XFramework.Component
{
    /// <summary>
    /// 手动阀门组件
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class ValveComponent : ComponentBase
    {
        private ValveState state = ValveState.OFF;
        /// <summary>
        /// 阀门状态（开（true）/关（false））
        /// </summary>
        public ValveState State
        {
            get
            {
                return state;
            }

            set
            {
                if (state == value)
                    return;
                state = value;
                OpenOrClose(state);
            }
        }

        /// <summary>
        /// 当前阀门初始角度
        /// </summary>
        private Vector3 originalRot;

        /// <summary>
        /// 旋转角度
        /// </summary>
        public Vector3 angle;

        /// <summary>
        /// 过渡时间
        /// </summary>
        public float duration = 1f;

        bool isDoing = false;

        void Awake()
        {
            originalRot = transform.eulerAngles;
        }

        /// <summary>
        /// 阀门是否打开
        /// </summary>
        /// <param name="_state"></param>
        public void OpenOrClose(ValveState _state)
        {
            if (isDoing)
            {
                return;
            }
            if (_state == ValveState.ON)
            {
                if (transform != null)
                {
                    isDoing = true;
                    state = _state;
                    transform.DORotate(angle, duration, RotateMode.LocalAxisAdd).OnComplete(() => {

                        isDoing = false;
                    });
                }


                EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, UUID, true);
            }
            else if (_state == ValveState.OFF)
            {
                if (transform != null)
                {
                    isDoing = true;
                    state = _state;
                    transform.DORotate(-angle, duration, RotateMode.LocalAxisAdd).OnComplete(() => {

                        isDoing = false;
                    });
                }


                EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, UUID, false);
            }
        }
    }
}
