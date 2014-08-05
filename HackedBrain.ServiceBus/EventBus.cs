using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;

namespace HackedBrain.ServiceBus.Core
{
	public class EventBus : IEventBus
	{
		#region Fields

		private IMessageSender messageSender;
		private IMessageReceiver messageReceiver;
		private IMessageSerializer messageSerializer;
		private IMessageMetadataProvider messageMetadataProvider;

		#endregion

		#region Constructors

		public EventBus(IMessageSender messageSender, IMessageReceiver messageReceiver, IMessageSerializer messageSerializer, IMessageMetadataProvider messageMetadataProvider)
		{
			this.messageSender = messageSender;
			this.messageReceiver = messageReceiver;
			this.messageSerializer = messageSerializer;
			this.messageMetadataProvider = messageMetadataProvider;
		}

		#endregion

		#region IEventBus implementation

		public Task PublishEventAsync<TEvent>(TEvent eventMessage)
		{
			IDictionary<string, object> serializedMessage = this.messageSerializer.Serialize<TEvent>(eventMessage);

			return this.messageSender.SendAsync(serializedMessage);
		}

		public IObservable<TEvent> WhenEventReceived<TEvent>()
		{
			Func<IDictionary<string, object>, bool> messageTypeFilter = this.messageMetadataProvider.GenerateMessageTypeFilter<TEvent>();
			
			return this.messageReceiver.WhenMessageReceived()
						.Where(serializedMessage => messageTypeFilter(serializedMessage))
						.Select(serializedMessage => this.messageSerializer.Deserialize<TEvent>(serializedMessage));

		}

		#endregion
	}
}
