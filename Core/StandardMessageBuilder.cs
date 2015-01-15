using System.Collections.Generic;

namespace HackedBrain.ServiceBus.Core
{
    public class StandardMessageBuilder : ICommandMessageBuilder, IEventMessageBuilder
    {
        #region IEventMessageBuilder implementation

        IMessage<TEvent> IEventMessageBuilder.BuildMessage<TEvent>(TEvent @event)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region ICommandMessageBuilder implementation

        IMessage<TCommand> ICommandMessageBuilder.BuildMessage<TCommand>(TCommand command)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
