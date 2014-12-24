using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
	public class CommandBus : ICommandBus
	{
		#region Fields

		private IMessageSender messageSender;
		private IMessageMetadataProvider messageMetadataProvider;

		#endregion

		#region Constructors

        public CommandBus(IMessageSender messageSender, IMessageMetadataProvider messageMetadataProvider)
		{
			this.messageSender = messageSender;
			this.messageMetadataProvider = messageMetadataProvider;
		}

		#endregion

		#region ICommandBus implementation

		public Task SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : class
		{
			if(command == null)
            {
                throw new ArgumentNullException("command");
            }
            
            IEnumerable<KeyValuePair<string, object>> metadata = this.messageMetadataProvider.GenerateMetadata<TCommand>(command);

            return this.messageSender.SendAsync(command, metadata, cancellationToken);
		}

		#endregion
	}
}
