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
			this.metadataProviders = metadataProviders;
		}

		#endregion

		#region IMessageMetadataProvider implementation

		public IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TMessage>(TMessage message) where TMessage : class
		{
			return this.metadataProviders.SelectMany(metadataProvider => metadataProvider.GenerateMetadata(message));
		}

		#endregion
	}
}
