
using System;
namespace HackedBrain.ServiceBus.Core
{
    public class StandardEventMessageBuilder : StandardMessageBuilder
    {
        #region Constructors

        public StandardEventMessageBuilder(IMessageMetadataProvider messageMetadataProvider) : base(messageMetadataProvider)
        {
        }

        #endregion

        #region IEventMessageBuilder implementation

        public IMessage<TEvent> BuildMessage<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if(@event == null)
            {
                throw new ArgumentNullException("event");
            }
            
            return this.BuildMessageInternal(@event);
        }

        #endregion
    }
}
