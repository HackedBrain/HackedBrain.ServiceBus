using System;
using System.Collections.Generic;
using System.IO;
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
        private IMessageBodySerializer messageBodySerializer;

        #endregion

        #region Constructors

        public ServiceBusMessageClientEntityMessageSender(MessageClientEntity messageClientEntity, Func<BrokeredMessage, Task> sendAsyncMethod, IMessageBodySerializer messageBodySerializer)
        {
            this.messageClientEntity = messageClientEntity;
            this.sendAsyncMethod = sendAsyncMethod;
            this.messageBodySerializer = messageBodySerializer;
        }

        #endregion

        #region IMessageSender implementation

        public Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            MemoryStream bodyStream = new MemoryStream();
            this.messageBodySerializer.SerializeBody(message.Body, bodyStream);

            BrokeredMessage brokeredMessage = new BrokeredMessage(bodyStream);
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
