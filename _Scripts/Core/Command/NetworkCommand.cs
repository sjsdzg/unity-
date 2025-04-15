using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    public class NetworkCommand : DelegateCommand<NetworkPackageInfo>
    {
        /// <summary>
        /// 注册类型
        /// </summary>
        public RegisterType Type { get; private set; }

        public NetworkCommand(string name, PackageHandler<NetworkPackageInfo> execution, RegisterType type = RegisterType.Request) : base(name, execution)
        {
            Type = type;
        }
    }
}
