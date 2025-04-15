using SuperSocket.ClientEngine.Protocol;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    /// <summary>
    /// Command basic interface
    /// </summary>
    /// <typeparam name="TPackageInfo">The type of the package info</typeparam>
    public interface ICommand<TPackageInfo> : ICommand
        where TPackageInfo : IPackageInfo
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="packageInfo">the package info</param>
        void ExecuteCommand(TPackageInfo packageInfo);
    }

}
