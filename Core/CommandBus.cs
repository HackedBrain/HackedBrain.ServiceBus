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

        public Task SendCommandAsync(Envelope<ICommand> commandEnvelope, CancellationToken cancellationToken)
        {
            if(commandEnvelope == null)
            {
                throw new ArgumentNullException("commandEnvelope");
            }

            IEnumerable<KeyValuePair<string, object>> metadata = this.messageMetadataProvider.GenerateMetadata(commandEnvelope.Body);

            return this.messageSender.SendAsync(commandEnvelope, metadata, cancellationToken);
        }

        public async Task SendCommandsAsync(IEnumerable<Envelope<ICommand>> commandEnvelopes, CancellationToken cancellationToken)
        {
            if(commandEnvelopes == null)
            {
                throw new ArgumentNullException("commandEnvelopes");
            }
            
            foreach(Envelope<ICommand> commandEnvelope in commandEnvelopes)
            {
                await this.SendCommandAsync(commandEnvelope, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        #endregion
    }
}
