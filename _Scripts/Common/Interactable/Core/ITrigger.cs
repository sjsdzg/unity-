using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    interface ITrigger<T>
    {
        void Trigger(T args0);
    }
}
