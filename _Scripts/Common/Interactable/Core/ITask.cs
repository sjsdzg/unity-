using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    interface ITask<T>
    {
        void Execute();
        void Next();
        void Completed();
        void Error(Exception error);
    }
}
