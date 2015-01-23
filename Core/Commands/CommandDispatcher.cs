using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public class CommandDispatcher : Dispatcher<ICommand>
    {
        public CommandDispatcher(IHandlerFactory commandHandlerFactory) : base(commandHandlerFactory)
        {
        }
    }
}
