using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
    public static class BrokeredMessageExtensions
    {
        public static IMessage ToMessage(this BrokeredMessage brokeredMessage, IMessageBodySerializer messageBodySerializer)
        {
            using(Stream messageBodyStream = brokeredMessage.GetBody<Stream>())
            {
                IEnumerable<KeyValuePair<string, object>> messageMetadata = brokeredMessage.Properties;
                
                object messageBody = messageBodySerializer.DeserializeBody(messageBodyStream, messageMetadata);
                
                return new Message(messageBody)
                    {

                        Id = brokeredMessage.MessageId,
                        CorrelationId = brokeredMessage.CorrelationId,
                        SessionId = brokeredMessage.SessionId,
                        Metadata = messageMetadata
                    };
            }
        }
    }
}
