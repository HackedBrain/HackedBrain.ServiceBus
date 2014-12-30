using System;
using System.Collections.Generic;

namespace HackedBrain.ServiceBus.Core
{
    public class StandardMessageMetadataProvider : IMessageMetadataProvider
    {
        #region Fields

        internal static readonly string MessageTypeKey = "StandardMessageMetadataProvider.MessageType";
        internal static readonly string CreatedOnKey = "StandardMessageMetadataProvider.CreatedOn";
        internal static readonly string ProviderVersionKey = "StandardMessageMetadataProvider.ProviderVersion";

        internal static readonly string ProviderVersionValue = typeof(StandardMessageMetadataProvider).Assembly.GetName().Version.ToString();

        #endregion

        #region IMessageMetadataProvider implementation

        public IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TMessage>(TMessage message) where TMessage : class
        {
            if(message == null)
            {
                throw new ArgumentNullException("message");     
            }

            return new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>(StandardMessageMetadataProvider.ProviderVersionKey, StandardMessageMetadataProvider.ProviderVersionValue),
                new KeyValuePair<string, object>(StandardMessageMetadataProvider.MessageTypeKey, typeof(TMessage).Name),
                new KeyValuePair<string, object>(StandardMessageMetadataProvider.CreatedOnKey, DateTime.UtcNow),
            };
        }

        #endregion
    }
}
