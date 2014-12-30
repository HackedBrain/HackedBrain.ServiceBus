using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
    public class ServiceBusTopicMessageSender : IMessageSender
    {
        #region Fields

        private TopicClient topicClient;
        private IMessageBodySerializer messageBodySerializer;		

        #endregion

        #region Constructors

        public ServiceBusTopicMessageSender(TopicClient topicClient, IMessageBodySerializer messageBodySerializer)
        {
            this.topicClient = topicClient;
            this.messageBodySerializer = messageBodySerializer;
        }

        #endregion

        #region IMessageSender implementation

        public Task SendAsync<TMessageBody>(TMessageBody body, IEnumerable<KeyValuePair<string, object>> metadata, CancellationToken cancellationToken) where TMessageBody : class
        {
            using(MemoryStream bodyStream = new MemoryStream())
            {
                this.messageBodySerializer.SerializeBody(body, bodyStream);

                BrokeredMessage brokeredMessage = new BrokeredMessage(bodyStream);
                brokeredMessage.MessageId = Guid.NewGuid().ToString("N");

                IMessageSessionIdProvider messageSessionIdProvider = body as IMessageSessionIdProvider;

                if(messageSessionIdProvider != null)
                {
                    brokeredMessage.SessionId = messageSessionIdProvider.SessionId;
                }

                foreach(KeyValuePair<string, object> entry in metadata)
                {
                    brokeredMessage.Properties.Add(entry);
                }

                cancellationToken.ThrowIfCancellationRequested();

                return this.topicClient.SendAsync(brokeredMessage);
            }
        }

        #endregion
    }
}
