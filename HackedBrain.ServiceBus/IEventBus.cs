using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus
{
	public interface IEventBus
	{
		Task PublishEventAsync<TEvent>(TEvent eventMessage);

		IObservable<TMessage> WhenEventReceived<TMessage>();
	}
}
