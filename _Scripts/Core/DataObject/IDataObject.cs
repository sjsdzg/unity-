using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    public interface IDataObject
    {

    }

    /// <summary>
    /// 数据类接口
    /// </summary>
    public interface IDataObject<T> : IDataObject where T : IDataObject<T>
    {

    }
}
