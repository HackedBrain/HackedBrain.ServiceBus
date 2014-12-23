using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
	public class CommandBus : ICommandBus
	{
		#region Fields

		private IMessageSender messageSender;
		private IMessageReceiver messageReceiver;
		private IMessageMetadataProvider messageMetadataProvider;

		#endregion

		#region Constructors

		public CommandBus(IMessageSender messageSender, IMessageReceiver messageReceiver, IMessageMetadataProvider messageMetadataProvider)
		{
			this.messageSender = messageSender;
			this.messageReceiver = messageReceiver;
			this.messageMetadataProvider = messageMetadataProvider;
		}

		#endregion

		#region ICommandBus implementation

		public Task SendCommandAsync<TCommand>(TCommand commandMessage, CancellationToken cancellationToken) where TCommand : class
		{
			IEnumerable<KeyValuePair<string, object>> metadata = this.messageMetadataProvider.GenerateMetadata<TCommand>(commandMessage);

            return this.messageSender.SendAsync(commandMessage, metadata, cancellationToken);
		}

		#endregion
	}
}
