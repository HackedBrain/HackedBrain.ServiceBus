using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
	public class EventBus : IEventBus
	{
		#region Fields

		private IMessageSender messageSender;
		private IMessageMetadataProvider messageMetadataProvider;

		#endregion

		#region Constructors

        public EventBus(IMessageSender messageSender, IMessageMetadataProvider messageMetadataProvider)
		{
			this.messageSender = messageSender;
			this.messageMetadataProvider = messageMetadataProvider;
		}

		#endregion

		#region IEventBus implementation

		public Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : class
		{
			if(@event == null)
            {
                throw new ArgumentNullException("event");
            }
            
            IEnumerable<KeyValuePair<string, object>> metadata = this.messageMetadataProvider.GenerateMetadata(@event);

			return this.messageSender.SendAsync<TEvent>(@event, metadata, cancellationToken);
		}

		#endregion
	}
}
