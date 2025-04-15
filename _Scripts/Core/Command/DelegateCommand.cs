using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    public delegate void PackageHandler<TPackageInfo>(TPackageInfo packageInfo);

    public class DelegateCommand<TPackageInfo> : ICommand<TPackageInfo>
        where TPackageInfo : IPackageInfo
    {
        private PackageHandler<TPackageInfo> m_Execution;

        public DelegateCommand(string name, PackageHandler<TPackageInfo> execution)
        {
            Name = name;
            m_Execution = execution;
        }

        public string Name { get; private set; }

        public void ExecuteCommand(TPackageInfo packageInfo)
        {
            m_Execution(packageInfo);
        }
    }
}
