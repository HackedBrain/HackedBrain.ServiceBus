using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackedBrain.ServiceBus.Core
{
	public interface IMessageReceiver
	{
		IObservable<IMessage> WhenMessageReceived();
	}
}
