using HackedBrain.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
    public class ServiceBusTopicMessageSender : ServiceBusMessageClientEntityMessageSender
    {
        #region Constructors

        public ServiceBusTopicMessageSender(TopicClient topicClient, IMessageBodySerializer messageBodySerializer) : base(topicClient, topicClient.SendAsync, messageBodySerializer)
        {
        }

        #endregion

    }
}
