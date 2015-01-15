using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public interface IMessageSender
    {
        Task SendAsync<TMessageBody>(Envelope<TMessageBody> envelope, IEnumerable<KeyValuePair<string, object>> metadata, CancellationToken cancellationToken);
    }
}
