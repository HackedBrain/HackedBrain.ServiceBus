using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using HackedBrain.ServiceBus.Core.Configuration;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure.Configuration
{
    public class AzureServiceBusMessageReceiverConfigurationBuilder : BaseServiceBusConfigurationBuilder
    {
        private string serviceBusNamespaceAddress;
        private TokenProvider tokenProvider;
        private string path;
        private IMessageBodySerializer messageBodySerializer;
        private bool withSessions;

        public AzureServiceBusMessageReceiverConfigurationBuilder(ServiceBusConfigurationBuilder serviceBusConfigurationBuilder, string serviceBusNamespaceAddress) : base(serviceBusConfigurationBuilder)
        {
            this.serviceBusNamespaceAddress = serviceBusNamespaceAddress;
        }

        public AzureServiceBusMessageReceiverConfigurationBuilder Path(string path)
        {
            this.path = path;
            
            return this;
        }

        public AzureServiceBusMessageReceiverConfigurationBuilder UsingTokenProvider(TokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;

            return this;
        }

        public AzureServiceBusMessageReceiverConfigurationBuilder UsingSharedAccessSignature(string keyName, string sharedAccessKey)
        {
            return this.UsingTokenProvider(TokenProvider.CreateSharedAccessSignatureTokenProvider(keyName, sharedAccessKey));
        }

        public AzureServiceBusMessageReceiverConfigurationBuilder UsingSharedAccessSignature(string sharedAccessSignature)
        {
            return this.UsingTokenProvider(TokenProvider.CreateSharedAccessSignatureTokenProvider(sharedAccessSignature));
        }

        public AzureServiceBusMessageReceiverConfigurationBuilder UsingMessageBodySerializer(IMessageBodySerializer messageBodySerializer)
        {
            this.messageBodySerializer = messageBodySerializer;
            
            return this;
        }

        public AzureServiceBusMessageReceiverConfigurationBuilder UsingMessageBodySerializer<TMessageBodySerializer>() where TMessageBodySerializer : IMessageBodySerializer, new()
        {
            return this.UsingMessageBodySerializer(new TMessageBodySerializer());
        }

        public AzureServiceBusMessageReceiverConfigurationBuilder WithSessions(bool withSessions = true)
        {
            this.withSessions = true;
            
            return this;
        }

        protected override void Build(ServiceBusConfiguration serviceBusConfiguration)
        {
            MessagingFactory messagingFactory = MessagingFactory.Create(
                this.serviceBusNamespaceAddress, 
                new MessagingFactorySettings
                { 
                    TokenProvider = this.tokenProvider,
                });

            IMessageReceiver messageReceiver;
            
            if(this.withSessions)
            {
                throw new NotImplementedException("Support for configuring session based message receivers not implemented yet.");
            }
            else
            {
                MessageReceiver azureMessageReceiver = messagingFactory.CreateMessageReceiver(this.path);

                messageReceiver = new ServiceBusMessageReceiverMessageReceiver(azureMessageReceiver, null);
            }

            serviceBusConfiguration.MessageReceivers.Add(messageReceiver);
        }
    }

    public static class AzureServiceBusServiceBusConfigurationBuilderExtensions
    {
        public static AzureServiceBusMessageReceiverConfigurationBuilder RegisterAzureServiceBusMessageReceiver(this ServiceBusConfigurationBuilder serviceBusConfigurationBuilder, string serviceBusNamespaceAddress)
        {
            return new AzureServiceBusMessageReceiverConfigurationBuilder(serviceBusConfigurationBuilder, serviceBusNamespaceAddress);
        }
    }
}
