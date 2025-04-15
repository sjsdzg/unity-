using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public interface IDriver
    {

    }

    public interface IUpdate : IDriver
    {
        void Update();
    }
}
