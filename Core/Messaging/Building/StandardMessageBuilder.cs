
using System;
using System.Collections.Generic;
using HackedBrain.ServiceBus.Core.Messaging;

namespace HackedBrain.ServiceBus.Core
{
    public class StandardMessageBuilder : IMessageBuilder
    {
        #region Fields

        private Func<Type, IMessageMetadataProvider> messageMetadataProviderFactory;
        private Func<Type, IMessageIdProvider> messageIdProviderFactory;
        private Func<Type, IMessageCorrelationIdProvider> messageCorrelationIdProviderFactory;
        private Func<Type, IMessageSessionIdProvider> messageSessionIdProviderFactory;

        #endregion

        #region Constructors

        public StandardMessageBuilder(Func<Type, IMessageMetadataProvider> messageMetadataProviderFactory, Func<Type, IMessageIdProvider> messageIdProviderFactory, Func<Type, IMessageCorrelationIdProvider> messageCorrelationIdProviderFactory, Func<Type, IMessageSessionIdProvider> messageSessionIdProviderFactory)
        {
            this.messageMetadataProviderFactory = messageMetadataProviderFactory;
            this.messageIdProviderFactory = messageIdProviderFactory;
            this.messageCorrelationIdProviderFactory = messageCorrelationIdProviderFactory;
            this.messageSessionIdProviderFactory = messageSessionIdProviderFactory;
        }

        #endregion

        #region IMessageBuilder implementation

        public IMessage<TBody> BuildMessage<TBody>(TBody body)
        {
            if(body == null)
            {
                throw new ArgumentNullException("body");
            }
            
            IEnumerable<KeyValuePair<string, object>> metadata = this.GenerateMetadata(body);
            string messageId = this.GenerateMessageId(body);
            string messageCorrelationId = this.GenerateCorrelationId(body);
            string sessionId = this.GenerateSessionId(body);

            return new Message<TBody>(body)
            {
                Id = messageId,
                CorrelationId = messageCorrelationId,
                SessionId = sessionId,
                Metadata = metadata
            };
        }

        #endregion

        #region Type specific methods

        protected virtual IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TBody>(TBody body)
        {
            IMessageMetadataProvider metadataProvider = this.messageMetadataProviderFactory(typeof(TBody));

            return metadataProvider != null ? metadataProvider.GenerateMetadata(body) : null;
        }

        protected virtual string GenerateMessageId<TBody>(TBody body)
        {
            IMessageIdProvider messageIdProvider = this.messageIdProviderFactory(typeof(TBody));
                
            return messageIdProvider != null ? messageIdProvider.GenerateMessageId(body) : null;
        }

        protected virtual string GenerateCorrelationId<TBody>(TBody body)
        {
            IMessageCorrelationIdProvider messageCorrelationIdProvider = this.messageCorrelationIdProviderFactory(typeof(TBody));

            return messageCorrelationIdProvider != null ? messageCorrelationIdProvider.GenerateCorrelationId(body) : null;
        }

        protected virtual string GenerateSessionId<TBody>(TBody body)
        {
            IMessageSessionIdProvider messageSessionIdProvider = this.messageSessionIdProviderFactory(typeof(TBody));

            return messageSessionIdProvider != null ? messageSessionIdProvider.GenerateSessionId(body) : null;
        }

        #endregion
    }

    public class FuncMessageMetadataProvider<TMessageBodyType> : IMessageMetadataProvider
    {
        Func<TMessageBodyType, IEnumerable<KeyValuePair<string, object>>> func;

        public FuncMessageMetadataProvider(Func<TMessageBodyType, IEnumerable<KeyValuePair<string, object>>> func)
        {

        }        

        public IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TBody>(TBody body)
        {
            if(typeof(TBody) != typeof(TMessageBodyType))
            {
                throw new ArgumentException(string.Format("This implementation only handles message bodies of type {0}, but was called for {1}.", typeof(TMessageBodyType).FullName, typeof(TBody).FullName));
            }

            return this.func((TMessageBodyType)(object)body);
        }
    }
}
