using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HackedBrain.ServiceBus.Core
{
    public class EventProcessor : MessageProcessor<IEvent>
    {
        #region Constructors

        public EventProcessor(IMessageReceiver messageReceiver, IDispatcher<IEvent> eventDispatcher) : base(messageReceiver, eventDispatcher)
        {
        }

        #endregion
    }
}
