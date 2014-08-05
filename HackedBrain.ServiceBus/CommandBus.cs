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
		private IMessageSerializer messageSerializer;
		private IMessageMetadataProvider messageMetadataProvider;

		#endregion

		#region Constructors

		public CommandBus(IMessageSender messageSender, IMessageReceiver messageReceiver, IMessageSerializer messageSerializer, IMessageMetadataProvider messageMetadataProvider)
		{
			this.messageSender = messageSender;
			this.messageReceiver = messageReceiver;
			this.messageSerializer = messageSerializer;
			this.messageMetadataProvider = messageMetadataProvider;
		}

		#endregion

		#region ICommandBus implementation

		public Task SendCommandAsync<TCommand>(TCommand commandMessage)
		{
			IDictionary<string, object> serializedMessage = this.messageSerializer.Serialize<TCommand>(commandMessage);

			this.messageMetadataProvider.ApplyMetadataToMessage<TCommand>(serializedMessage);

			return this.messageSender.SendAsync(serializedMessage);
		}

		public IObservable<TCommand> WhenCommandReceived<TCommand>()
		{
			Func<IDictionary<string, object>, bool> messageTypeFilter = this.messageMetadataProvider.GenerateMessageTypeFilter<TCommand>();
			
			return this.messageReceiver.WhenMessageReceived()
						.Where(serializedMessage => messageTypeFilter(serializedMessage))
						.Select(serializedMessage => this.messageSerializer.Deserialize<TCommand>(serializedMessage));


		}

		#endregion
	}
}
