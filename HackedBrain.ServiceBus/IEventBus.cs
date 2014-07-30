using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus
{
	public interface IEventBus
	{
		Task PublishAsync<TEvent>(TEvent eventMessage);

		IObservable<TMessage> Listen<TMessage>();
	}
}
