using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;
using XFramework.Module;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class UpdateBoolAction : ActionBase
    {
        /// <summary>
        /// 变量
        /// </summary>
        private bool variable;

        /// <summary>
        /// 是否匹配
        /// </summary>
        public bool value;

        public UpdateBoolAction(ref bool _variable, bool _value)
        {
            variable = _variable;
            value = _value;
        }

        public override void Execute()
        {
            variable = value;
            Completed();
        }
    }
}
