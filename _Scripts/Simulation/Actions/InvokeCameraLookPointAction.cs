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
    public class InvokeCameraLookPointAction : ActionBase
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
        public InvokeCameraLookPointAction(string _LookPoint, float _time,bool _Immediate = true)
        {
            lookPoint = _LookPoint;
            m_Time = _time;
            immediate = _Immediate;
        }
        public override void Execute()
        {
            LookPointManager.Instance.Enter(lookPoint,()=> {
                Completed();
            });          
            //CameraLookPointManager.Instance.InvokeCameraLookPoint(lookPoint);
            //Camera.main.GetComponent<CameraSwitcher>().enabled = false;
            //EventDispatcher.ExecuteEvent(Events.Prompt.Hide);
            //if (immediate)
            //{
            //    Completed();
            //    Task.NewTask()
            //        .Append(new DelayedAction(m_Time))
            //        .OnCompleted(() => {
            //            Camera.main.GetComponent<CameraSwitcher>().enabled = true;
            //            EventDispatcher.ExecuteEvent(Events.Prompt.Show);

            //        }).Execute();
            //}
            //else
            //{
            //    Task.NewTask()
            //        .Append(new DelayedAction(m_Time))
            //        .OnCompleted(() => {
            //            Camera.main.GetComponent<CameraSwitcher>().enabled = true;
            //            EventDispatcher.ExecuteEvent(Events.Prompt.Show);

            //            Completed();
            //        }).Execute();
            //}
        }
    }
}
