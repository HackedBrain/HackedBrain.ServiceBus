using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
    public class ServiceBusMessageClientEntityMessageSender : IMessageSender
    {
        #region Fields

        private MessageClientEntity messageClientEntity;
        private Func<BrokeredMessage, Task> sendAsyncMethod;

        #endregion

        #region Constructors

        public ServiceBusMessageClientEntityMessageSender(MessageClientEntity messageClientEntity, Func<BrokeredMessage, Task> sendAsyncMethod)
        {
            this.messageClientEntity = messageClientEntity;
            this.sendAsyncMethod = sendAsyncMethod;
        }

        #endregion

        #region IMessageSender implementation

        public Task SendAsync<TMessageBody>(Envelope<TMessageBody> envelope, IEnumerable<KeyValuePair<string, object>> metadata, CancellationToken cancellationToken)
        {
            BrokeredMessage brokeredMessage = new BrokeredMessage(envelope.Body);
            brokeredMessage.MessageId = envelope.MessageId ?? Guid.NewGuid().ToString("N");

            if(envelope.CorrelationId != null)
            {
                brokeredMessage.CorrelationId = envelope.CorrelationId;
            }

            if(envelope.SessionId != null)
            {
                brokeredMessage.SessionId = envelope.SessionId;
            }

            foreach(KeyValuePair<string, object> entry in metadata)
            {
                brokeredMessage.Properties.Add(entry);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return this.sendAsyncMethod(brokeredMessage);
        }

        #endregion
    }
}
