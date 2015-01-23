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
        public static IMessage<TMessageBody> ToMessage<TMessageBody>(this BrokeredMessage brokeredMessage, IMessageBodySerializer messageBodySerializer)
        {
            using(Stream messageBodyStream = brokeredMessage.GetBody<Stream>())
            {
                TMessageBody messageBody = messageBodySerializer.DeserializeBody<TMessageBody>(messageBodyStream);
                
                return new Message<TMessageBody>(messageBody)
                    {

                        Id = brokeredMessage.MessageId,
                        CorrelationId = brokeredMessage.CorrelationId,
                        SessionId = brokeredMessage.SessionId,
                        Metadata = brokeredMessage.Properties
                    };
            }
        }
    }
}
