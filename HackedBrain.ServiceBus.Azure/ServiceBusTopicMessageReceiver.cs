using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
	public class ServiceBusTopicMessageReceiver : IMessageReceiver
	{
		#region Fields

		SubscriptionClient subscriptionClient;		

		#endregion

		#region Constructors

		public ServiceBusTopicMessageReceiver(SubscriptionClient subscriptionClient)
		{
			this.subscriptionClient = subscriptionClient;
		}

		#endregion
			
		#region IMessageReceiver implementation

		public IObservable<IDictionary<string,object>> WhenMessageReceived()
		{
			throw new NotImplementedException("TODO: bring HackedBrain.ServiceBus into play here.");	
		}

		#endregion
}
}
