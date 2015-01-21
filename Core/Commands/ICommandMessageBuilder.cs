using System.Collections.Generic;

namespace HackedBrain.ServiceBus.Core
{
	public interface ICommandMessageBuilder
	{
		IMessage<TCommand> BuildMessage<TCommand>(TCommand command) where TCommand : ICommand;
	}
}
