
using System;
using System.Collections.Generic;
using HackedBrain.ServiceBus.Core.Messaging;

namespace HackedBrain.ServiceBus.Core
{
    public class StandardMessageBuilder : IMessageBuilder
    {
        #region Fields

        private Func<Type, IMessageIdProvider> messageIdProviderFactory;
        private Func<Type, IMessageCorrelationIdProvider> messageCorrelationIdProviderFactory;
        private Func<Type, IMessageSessionIdProvider> messageSessionIdProviderFactory;
        private Func<Type, IMessageMetadataProvider> messageMetadataProviderFactory;

        #endregion

        #region Constructors

        public StandardMessageBuilder(Func<Type, IMessageIdProvider> messageIdProviderFactory, Func<Type, IMessageCorrelationIdProvider> messageCorrelationIdProviderFactory, Func<Type, IMessageSessionIdProvider> messageSessionIdProviderFactory, Func<Type, IMessageMetadataProvider> messageMetadataProviderFactory)
        {
            this.messageIdProviderFactory = messageIdProviderFactory;
            this.messageCorrelationIdProviderFactory = messageCorrelationIdProviderFactory;
            this.messageSessionIdProviderFactory = messageSessionIdProviderFactory;
            this.messageMetadataProviderFactory = messageMetadataProviderFactory;
        }

        #endregion

        #region IMessageBuilder implementation

        public IMessage<TBody> BuildMessage<TBody>(TBody body)
        {
            if(body == null)
            {
                throw new ArgumentNullException("body");
            }

            return new Message<TBody>(body)
            {
                Id = this.GenerateMessageId(body),
                CorrelationId = this.GenerateCorrelationId(body),
                SessionId = this.GenerateSessionId(body),
                Metadata = this.GenerateMetadata(body)
            };
        }

        #endregion

        #region Type specific methods

        protected virtual IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TBody>(TBody body)
        {
            return this.GetValueOrDefaultFromMessageBody(
                this.messageMetadataProviderFactory,
                body,
                (p, b) => p.GenerateMetadata(b));
        }

        protected virtual string GenerateMessageId<TBody>(TBody body)
        {
            return this.GetValueOrDefaultFromMessageBody(
                this.messageIdProviderFactory,
                body,
                (p, b) => p.GenerateMessageId(b));
        }

        protected virtual string GenerateCorrelationId<TBody>(TBody body)
        {
            return this.GetValueOrDefaultFromMessageBody(
                this.messageCorrelationIdProviderFactory,
                body,
                (p, b) => p.GenerateCorrelationId(b));
        }

        protected virtual string GenerateSessionId<TBody>(TBody body)
        {
            return this.GetValueOrDefaultFromMessageBody(
                this.messageSessionIdProviderFactory,
                body,
                (p, b) => p.GenerateSessionId(b));
        }

        #endregion

        #region Helper methods

        private TResult GetValueOrDefaultFromMessageBody<TBody, TThinger, TResult>(Func<Type, TThinger> thingers, TBody body, Func<TThinger, TBody, TResult> func) where TThinger : class
        {
            TThinger thinger = thingers(typeof(TThinger));

            return thinger != null ? func(thinger, body) : default(TResult);
        }

        #endregion
    }

    public class FuncMessageMetadataProvider<TMessageBodyType> : IMessageMetadataProvider
    {
        Func<TMessageBodyType, IEnumerable<KeyValuePair<string, object>>> providerFunc;

        public FuncMessageMetadataProvider(Func<TMessageBodyType, IEnumerable<KeyValuePair<string, object>>> providerFunc)
        {
            this.providerFunc = providerFunc;
        }

        public IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TBody>(TBody body)
        {
            if(typeof(TBody) != typeof(TMessageBodyType))
            {
                throw new ArgumentException(string.Format("This implementation only handles message bodies of type {0}, but was called for {1}.", typeof(TMessageBodyType).FullName, typeof(TBody).FullName));
            }

            return this.providerFunc((TMessageBodyType)(object)body);
        }
    }
}
