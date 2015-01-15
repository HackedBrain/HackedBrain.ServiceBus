using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public class EventBus : IEventBus
    {
        #region Fields

        private IMessageSender messageSender;
        private IMessageMetadataProvider messageMetadataProvider;

        #endregion

        #region Constructors

        public EventBus(IMessageSender messageSender, IMessageMetadataProvider messageMetadataProvider)
        {
            this.messageSender = messageSender;
            this.messageMetadataProvider = messageMetadataProvider;
        }

        #endregion

        #region IEventBus implementation

        public Task PublishEventAsync(Envelope<IEvent> eventEnvelope, CancellationToken cancellationToken)
        {
            if(eventEnvelope == null)
            {
                throw new ArgumentNullException("eventEnvelope");
            }
            
            IEnumerable<KeyValuePair<string, object>> metadata = this.messageMetadataProvider.GenerateMetadata(eventEnvelope.Body);

            return this.messageSender.SendAsync(eventEnvelope, metadata, cancellationToken);
        }

        public async Task PublishEventsAsync(IEnumerable<Envelope<IEvent>> eventEnvelopes, CancellationToken cancellationToken)
        {
            if(eventEnvelopes == null)
            {
                throw new ArgumentNullException("eventEnvelopes");
            }
            
            foreach(Envelope<IEvent> eventEnvelope in eventEnvelopes)
            {
                await this.PublishEventAsync(eventEnvelope, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        #endregion
    }
}
