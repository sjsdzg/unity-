using SuperSocket.ClientEngine;
using SuperSocket.ClientEngine.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    /// <summary>
    /// the empty basic interface for command loader
    /// </summary>
    public interface ICommandLoader
    {
    }

    /// <summary>
    /// Command loader's interface
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandLoader<TCommand> : ICommandLoader
        where TCommand : ICommand
    {
        /// <summary>
        /// Initializes the command loader.
        /// </summary>
        /// <returns></returns>
        bool Initialize();

        /// <summary>
        /// Tries to load commands
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        bool TryLoadCommands(out IEnumerable<TCommand> commands);

        /// <summary>
        /// Occurs when [error].
        /// </summary>
        event EventHandler<ErrorEventArgs> Error;
    }
}
