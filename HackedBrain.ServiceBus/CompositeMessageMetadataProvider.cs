﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public Func<IDictionary<string, object>, bool> GenerateMessageTypeFilter<TMessage>() where TMessage : class
		{
			return metadata => metadata[StandardMessageMetadataProvider.MessageTypeKey].Equals(typeof(TMessage).Name);
		}

		public IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TMessage>(TMessage message) where TMessage : class
		{
			return this.metadataProviders.SelectMany(metadataProvider => metadataProvider.GenerateMetadata(message));
		}

		#endregion
	}
}