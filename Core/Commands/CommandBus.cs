using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core.Messaging;

namespace HackedBrain.ServiceBus.Core
{
    public class CommandBus : ICommandBus
    {
        #region Fields

        private IMessageSender messageSender;
        private IMessageBuilder messageBuilder;

        #endregion

        #region Constructors

        public CommandBus(IMessageBuilder messageBuilder, IMessageSender messageSender)
        {
            this.messageBuilder = messageBuilder;
            this.messageSender = messageSender;
        }

        #endregion

        #region ICommandBus implementation

        public Task SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand
        {
            if(command == null)
            {
                throw new ArgumentNullException("commandEnvelope");
            }

            if(cancellationToken == null)
            {
                throw new ArgumentNullException("cancellationToken");
            }

            IMessage message = this.messageBuilder.BuildMessage(command);

            return this.messageSender.SendAsync(message, cancellationToken);
        }

        public async Task SendCommandsAsync(IEnumerable<ICommand> commands, CancellationToken cancellationToken)
        {
            if(commands == null)
            {
                throw new ArgumentNullException("commandEnvelopes");
            }

            if(cancellationToken == null)
            {
                throw new ArgumentNullException("cancellationToken");
            }
            
            foreach(ICommand commandEnvelope in commands)
            {
                await this.SendCommandAsync(commandEnvelope, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        #endregion
    }
}
