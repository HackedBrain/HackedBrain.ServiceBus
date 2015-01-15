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

        public Task SendAsync<TMessageBody>(IMessage<TMessageBody> message, CancellationToken cancellationToken)
        {
            BrokeredMessage brokeredMessage = new BrokeredMessage(message.Body);
            brokeredMessage.MessageId = message.Id ?? Guid.NewGuid().ToString("N");

            if(message.CorrelationId != null)
            {
                brokeredMessage.CorrelationId = message.CorrelationId;
            }

            if(message.SessionId != null)
            {
                brokeredMessage.SessionId = message.SessionId;
            }

            foreach(KeyValuePair<string, object> entry in message.Metadata)
            {
                brokeredMessage.Properties.Add(entry);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return this.sendAsyncMethod(brokeredMessage);
        }

        #endregion
    }
}
