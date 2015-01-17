using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private ICommandHandlerFactory commandHandlerFactory;
        private ConcurrentDictionary<Type, Func<ICommand, Task>> commandHandlerDispatchers = new ConcurrentDictionary<Type, Func<ICommand, Task>>();


        public CommandDispatcher(ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public Task DispatchAsync(ICommand command, CancellationToken cancellationToken)
        {
            Type commandType = command.GetType();

            return this.GetDispatcherForCommand(commandType)(command);
        }

        public Func<ICommand, Task> GetDispatcherForCommand(Type commandType)
        {
            return this.commandHandlerDispatchers.GetOrAdd(
                commandType,
                ct =>
                {
                    // TODO: optimize this gross, non-performant reflection/dynamic logic by building dynamic lambda with expression API
                    return command =>
                        {
                            ICommandHandler handler = (ICommandHandler)this.commandHandlerFactory.GetType().GetMethod("CreateHandler").GetGenericMethodDefinition().MakeGenericMethod(ct).Invoke(this.commandHandlerFactory, null);

                            return (Task)((dynamic)handler).Handle(command);
                        };
                });
        }
    }
}
