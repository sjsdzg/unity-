using SuperSocket.ClientEngine.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using XFramework.Common;

namespace XFramework.Core
{
    public class ReflectCommandLoader<TCommand> : CommandLoaderBase<TCommand>
        where TCommand : class, ICommand
    {
        /// <summary>
        /// Initializes a new instance of the ReflectCommandLoader class.
        /// </summary>
        /// <returns></returns>
        public ReflectCommandLoader()
        {

        }
        /// <summary>
        /// Initializes the command loader
        /// </summary>
        /// <returns></returns>
        public override bool Initialize()
        {
            return true;
        }

        /// <summary>
        /// Tris to load commands
        /// </summary>
        /// <param name="commands">The commands</param>
        /// <returns></returns>
        public override bool TryLoadCommands(out IEnumerable<TCommand> commands)
        {
            commands = null;
            var assembly = Assembly.GetExecutingAssembly();

            var outputCommands = new List<TCommand>();

            try
            {
                outputCommands.AddRange(assembly.GetImplementedObjectsByInterface<TCommand>());
            }
            catch (Exception e)
            {
                OnError(new Exception(string.Format("Failed to get commands from the assembly {0}!", assembly.FullName), e));
                return false;
            }

            commands = outputCommands;

            return true;
        }
    }
}
