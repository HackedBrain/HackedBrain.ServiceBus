using System;
using System.Collections.Generic;

namespace HackedBrain.ServiceBus.Core.Configuration
{
    public class ServiceBusConfiguration
    {
        public ServiceBusConfiguration()
        {
            this.MessageReceivers = new List<IMessageReceiver>();
            this.MessageSenders = new List<IMessageSender>();
        }

        public List<IMessageReceiver> MessageReceivers
        {
            get;
            private set;
        }

        public List<IMessageSender> MessageSenders
        {
            get;
            private set;
        }
    }

    public class ServiceBusConfigurationBuilder
    {
        List<Action<ServiceBusConfiguration>> builderCallbacks = new List<Action<ServiceBusConfiguration>>();

        public void RegisterBuilderCallback(Action<ServiceBusConfiguration> builderCallback)
        {
            builderCallbacks.Add(builderCallback);
        }

        public ServiceBusConfiguration Build()
        {
            ServiceBusConfiguration result = new ServiceBusConfiguration();
            
            foreach(Action<ServiceBusConfiguration> builderCallback in this.builderCallbacks)
            {
                builderCallback(result);
            }

            return result;
        }
    }

    public abstract class BaseServiceBusConfigurationBuilder
    {
        protected BaseServiceBusConfigurationBuilder(ServiceBusConfigurationBuilder serviceBusConfigurationBuilder)
        {
            serviceBusConfigurationBuilder.RegisterBuilderCallback(this.Build);
        }

        protected abstract void Build(ServiceBusConfiguration serviceBusConfiguration);
    }
}
