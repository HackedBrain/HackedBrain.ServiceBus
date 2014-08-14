using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus
{
	public interface IEventBus
	{
		Task PublishEventAsync<TEvent>(TEvent eventMessage) where TEvent : class;

		IObservable<TMessage> WhenEventReceived<TMessage>() where TMessage : class;
	}
}
