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

		public Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : class
		{
			IDictionary<string, object> metadata = this.messageMetadataProvider.GenerateMetadata(@event);

			return this.messageSender.SendAsync<TEvent>(@event, metadata);
		}

		public IObservable<TEvent> WhenEventReceived<TEvent>() where TEvent : class
		{
			Func<IDictionary<string, object>, bool> messageTypeFilter = this.messageMetadataProvider.GenerateMessageTypeFilter<TEvent>();
			
			return this.messageReceiver.WhenMessageReceived()
						.Where(message => messageTypeFilter(message.Metadata))
						.Select(message => message.GetBody<TEvent>());

		}

		#endregion
	}
}
