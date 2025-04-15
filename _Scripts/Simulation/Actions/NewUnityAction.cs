using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Common;

namespace XFramework.Common
{
    /// <summary>
    /// GameOjectAction
    /// </summary>
    public class NewUnityAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public UnityAction action;

        /// <summary>
        /// 状态
        /// </summary>
        public bool activeState;

        public NewUnityAction(UnityAction _action)
        {
            action = _action;
        }
        /// <summary>
        /// 执行
        /// </summary>
        public override void Execute()
        {
            action.Invoke();
            Completed();
        }
    }
    
}
