
using System.Collections.Generic;
namespace HackedBrain.ServiceBus.Core
{
    public abstract class StandardMessageBuilder
    {
        #region Fields

        private IMessageMetadataProvider messageMetadataProvider;

        #endregion

        #region Constructors

        public StandardMessageBuilder(IMessageMetadataProvider messageMetadataProvider)
        {
            this.messageMetadataProvider = messageMetadataProvider;
        }

        #endregion

        #region Type specific methods

        protected virtual Message<T> BuildMessageInternal<T>(T body)
        {
            IEnumerable<KeyValuePair<string, object>> metadata = this.messageMetadataProvider.GenerateMetadata(body);
            
            return new Message<T>(body)
            {
                Metadata = metadata
            };
        }

        #endregion
    }
}
