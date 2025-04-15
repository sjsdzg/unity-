using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    /// <summary>
    /// 撤销/重做 记录接口
    /// </summary>
    public interface IUndoRecord
    {

        string Name { get; }


        void Execute();
    }
}
