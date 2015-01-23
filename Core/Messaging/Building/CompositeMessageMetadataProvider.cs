using System;
using System.Collections.Generic;
using System.Linq;

namespace HackedBrain.ServiceBus.Core
{
	public class CompositeMessageMetadataProvider : IMessageMetadataProvider
	{
		#region Fields

		private readonly IEnumerable<IMessageMetadataProvider> metadataProviders;

		#endregion

		#region Constructors

		public CompositeMessageMetadataProvider(IEnumerable<IMessageMetadataProvider> metadataProviders)
		{
			if(metadataProviders == null)
            {
                throw new ArgumentNullException("metadataProviders");
            }
            
            this.metadataProviders = metadataProviders;
		}

		#endregion

		#region IMessageMetadataProvider implementation

		public IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TMessage>(TMessage message)
		{
			if(message == null)
            {
                throw new ArgumentNullException("message");
            }
            
            return this.metadataProviders.SelectMany(metadataProvider => metadataProvider.GenerateMetadata(message));
		}

		#endregion
	}
}
