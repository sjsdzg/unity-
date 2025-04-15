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
    public class UpdateSmallAction : ActionBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 检测状态
        /// </summary>
        public bool status;
        public UpdateSmallAction(string _name,bool _status)
        {
            name = _name;
            status = _status;
        }
        public override void Execute()
        {
            SmallActionManager.Instance.UpdateSmallAction(name, status);
            Completed();
        }
    }
}
