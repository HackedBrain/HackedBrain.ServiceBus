using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core.Messaging;

namespace HackedBrain.ServiceBus.Core
{
    public class EventBus : IEventBus
    {
        #region Fields

        private IMessageSender messageSender;
        private IMessageBuilder messageBuilder;

        #endregion

        #region Constructors

        public EventBus(IMessageBuilder messageBuilder, IMessageSender messageSender)
        {
            this.messageBuilder = messageBuilder;
            this.messageSender = messageSender;
        }

        #endregion

        #region IEventBus implementation

        public Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent
        {
            if(@event == null)
            {
                throw new ArgumentNullException("event");
            }

            if(cancellationToken == null)
            {
                throw new ArgumentNullException("cancellationToken");
            }

            IMessage<TEvent> message = this.messageBuilder.BuildMessage(@event);

            return this.messageSender.SendAsync(message, cancellationToken);
        }

        public async Task PublishEventsAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken)
        {
            if(events == null)
            {
                throw new ArgumentNullException("eventEnvelopes");
            }

            if(cancellationToken == null)
            {
                throw new ArgumentNullException("cancellationToken");
            }
            
            foreach(IEvent @event in events)
            {
                await this.PublishEventAsync(@event, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        #endregion
    }
}
