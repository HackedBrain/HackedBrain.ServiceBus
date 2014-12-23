using System;

namespace HackedBrain.ServiceBus.Core
{
	public interface IMessageReceiver
	{
		IObservable<IMessage> WhenMessageReceived();
	}
}
