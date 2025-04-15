using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework.Actions
{
    public class InvokeCameraLookAndMovePointAction : ActionBase
    {
        /// <summary>
        /// 主相机观察点
        /// </summary>
        private string lookPoint;

        /// <summary>
        /// 镜头时长
        /// </summary>
        private float m_Time;

        /// <summary>
        /// 是否立刻返回完成
        /// </summary>
        public bool immediate;
        /// <summary>
        /// 镜头位移目标点
        /// </summary>
        public InvokeCameraLookAndMovePointAction(string _LookPoint, float _time, bool _Immediate = true)
        {
            lookPoint = _LookPoint;
            m_Time = _time;
            immediate = _Immediate;
        }
        public override void Execute()
        {
            CameraLookPointManager.Instance.InvokeCameraLookPoint(lookPoint, m_Time);
            Camera.main.GetComponent<CameraSwitcher>().Switch(CameraStyle.Free);
            if (immediate)
            {
                Completed();
            }
            else
            {
                Task.NewTask()
                    .Append(new DelayedAction(m_Time))
                    .OnCompleted(() =>
                    {
                        Completed();
                    }).Execute();
            }
        }
    }
}
