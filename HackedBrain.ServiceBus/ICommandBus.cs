using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus
{
	public interface ICommandBus
	{
		Task SendCommandAsync<TCommand>(TCommand commandMessage);

		IObservable<TCommand> WhenCommandReceived<TCommand>();
	}
}
