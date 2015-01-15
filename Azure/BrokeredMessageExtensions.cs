using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
    public static class BrokeredMessageExtensions
    {
        public static IMessage<TMessageBody> ToMessage<TMessageBody>(this BrokeredMessage brokeredMessage)
        {
            return new Message<TMessageBody>(brokeredMessage.GetBody<TMessageBody>())
                {

                    Id = brokeredMessage.MessageId,
                    CorrelationId = brokeredMessage.CorrelationId,
                    SessionId = brokeredMessage.SessionId,
                    Metadata = brokeredMessage.Properties
                };
        }
    }
}
