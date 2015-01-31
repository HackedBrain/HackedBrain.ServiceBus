using System;
using System.Collections.Generic;
using HackedBrain.ServiceBus.Core;

namespace HackedBrain.ServiceBus.Core
{
    public class StandardMessageMetadataProvider : IMessageMetadataProvider
    {
        #region Fields

        internal static readonly string MessageTypeKey = "StandardMessageMetadataProvider.MessageType";
        internal static readonly string ProviderVersionKey = "StandardMessageMetadataProvider.ProviderVersion";

        internal static readonly string ProviderVersionValue = typeof(StandardMessageMetadataProvider).Assembly.GetName().Version.ToString();
        
        private Func<Type, IMessageTypeNameProvider> messageTypeNameProviderFactory;

        #endregion

        #region Constructors

        public StandardMessageMetadataProvider(Func<Type, IMessageTypeNameProvider> messageTypeNameProviderFactory)
        {
            this.messageTypeNameProviderFactory = messageTypeNameProviderFactory;
        }

        #endregion

        #region IMessageMetadataProvider implementation

        public virtual IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TMessageBody>(TMessageBody body)
        {
            if(body == null)
            {
                throw new ArgumentNullException("message");     
            }

            return new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>(StandardMessageMetadataProvider.ProviderVersionKey, StandardMessageMetadataProvider.ProviderVersionValue),
                new KeyValuePair<string, object>(StandardMessageMetadataProvider.MessageTypeKey, this.GenerateMessageTypeName<TMessageBody>(body)),
            };
        }

        private string GenerateMessageTypeName<TMessageBody>(TMessageBody body)
        {
            Type messageBodyType = typeof(TMessageBody);
            IMessageTypeNameProvider typeNameProvider = this.messageTypeNameProviderFactory(messageBodyType);

            return typeNameProvider != null ? typeNameProvider.GenerateMessageTypeName<TMessageBody>(body) : messageBodyType.Name;
        }

        #endregion
    }
}
