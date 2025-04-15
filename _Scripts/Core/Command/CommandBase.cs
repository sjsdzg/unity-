using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    /// <summary>
    /// Command base class
    /// </summary>
    /// <typeparam name="TPackageInfo">The type fo the package info.</typeparam>
    public abstract class CommandBase<TPackageInfo> : ICommand<TPackageInfo>
        where TPackageInfo : IPackageInfo
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public virtual string Name
        {
            get { return this.GetType().Name; }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="packageInfo">the package info.</param>
        public abstract void ExecuteCommand(TPackageInfo packageInfo);

        /// <summary>
        /// Returns a that represents this instance.
        /// </summary>
        /// <returns>A that represents this instance.</returns>
        public override string ToString()
        {
            return this.GetType().AssemblyQualifiedName;
        }
    }
}
