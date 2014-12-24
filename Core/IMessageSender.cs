using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
	public interface IMessageSender
	{
        Task SendAsync<TMessageBody>(TMessageBody body, IEnumerable<KeyValuePair<string, object>> metadata, CancellationToken cancellationToken) where TMessageBody : class;
	}
}
