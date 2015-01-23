using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HackedBrain.ServiceBus.Core
{
    public class CommandProcessor : MessageProcessor<ICommand>
    {
        #region Constructors

        public CommandProcessor(IMessageReceiver messageReceiver, IDispatcher<ICommand> commandDispatcher) : base(messageReceiver, commandDispatcher)
        {
        }

        #endregion
    }
}
