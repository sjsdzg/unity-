using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    /// <summary>
    /// 状态改变事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="newState"></param>
    /// <param name="oldState"></param>
    public delegate void StateChangedEventHandler(object sender, EnumObjectState newState, EnumObjectState oldState);
}
