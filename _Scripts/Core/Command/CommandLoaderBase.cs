using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.ClientEngine;
using SuperSocket.ClientEngine.Protocol;

namespace XFramework.Core
{
    public abstract class CommandLoaderBase<TCommand> : ICommandLoader<TCommand>
        where TCommand : ICommand
    {
        /// <summary>
        /// Initializes the command loader.
        /// </summary>
        /// <returns></returns>
        public abstract bool Initialize();

        /// <summary>
        /// Tries to load commands.
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public abstract bool TryLoadCommands(out IEnumerable<TCommand> commands);

        /// <summary>
        /// Call when [error].
        /// </summary>
        /// <param name="message">The Message.</param>
        protected void OnError(string message)
        {
            OnError(new Exception(message));
        }

        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="e">The e.</param>
        protected void OnError(Exception e)
        {
            var handler = Error;

            if (handler != null)
            {
                handler(this, new ErrorEventArgs(e));
            }
        }

        /// <summary>
        /// Occurs when [error].
        /// </summary>
        public event EventHandler<ErrorEventArgs> Error;
    }
}
