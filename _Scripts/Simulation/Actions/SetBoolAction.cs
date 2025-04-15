using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Simulation
{
    /// <summary>
    /// 设置 bool
    /// </summary>
    public class SetBoolAction : ActionBase
    {
        private bool setPams;

        private bool setBool;

        public SetBoolAction(bool _pams,bool _setBool)
        {
            setPams = _pams;
            setBool = _setBool;

        }
        public override void Execute()
        {
            setPams = setBool;
            Completed();
            //behaviour.Invoke(time, Completed);
        }

        public override void Completed()
        {
            base.Completed();

        }
    }
}
