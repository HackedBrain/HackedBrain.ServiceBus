using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

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

		public Task SendCommandAsync<TCommand>(TCommand commandMessage) where TCommand : class
		{
			IDictionary<string, object> metadata = this.messageMetadataProvider.GenerateMetadata<TCommand>(commandMessage);

			return this.messageSender.SendAsync(commandMessage, metadata);
		}

		public IObservable<TCommand> WhenCommandReceived<TCommand>() where TCommand : class
		{
			Func<IDictionary<string, object>, bool> messageTypeFilter = this.messageMetadataProvider.GenerateMessageTypeFilter<TCommand>();
			
			return this.messageReceiver.WhenMessageReceived()
						.Where(message => messageTypeFilter(message.Metadata))
						.Select(message => message.GetBody<TCommand>());


		}

		#endregion
	}
}
