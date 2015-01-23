
using System;
namespace HackedBrain.ServiceBus.Core
{
    public class StandardCommandMessageBuilder : StandardMessageBuilder
    {
        #region Constructors

        public StandardCommandMessageBuilder(IMessageMetadataProvider messageMetadataProvider) : base(messageMetadataProvider)
        {
        }

        #endregion

        #region ICommandMessageBuilder implementation

        public IMessage<TCommand> BuildMessage<TCommand>(TCommand command) where TCommand : ICommand
        {
            if(command == null)
            {
                throw new ArgumentNullException("command");
            }

            return this.BuildMessageInternal(command);
        }

        #endregion
    }
}
