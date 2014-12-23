using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
	public class EventBus : IEventBus
	{
		#region Fields

		private IMessageSender messageSender;
		private IMessageReceiver messageReceiver;
		private IMessageMetadataProvider messageMetadataProvider;

		#endregion

		#region Constructors

		public EventBus(IMessageSender messageSender, IMessageReceiver messageReceiver, IMessageMetadataProvider messageMetadataProvider)
		{
			this.messageSender = messageSender;
			this.messageReceiver = messageReceiver;
			this.messageMetadataProvider = messageMetadataProvider;
		}

		#endregion

		#region IEventBus implementation

		public Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : class
		{
			IEnumerable<KeyValuePair<string, object>> metadata = this.messageMetadataProvider.GenerateMetadata(@event);

			return this.messageSender.SendAsync<TEvent>(@event, metadata, cancellationToken);
		}

		#endregion
	}
}
