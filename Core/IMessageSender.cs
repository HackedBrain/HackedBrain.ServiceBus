using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public interface IMessageSender
    {
        Task SendAsync<TMessageBody>(IMessage<TMessageBody> message, CancellationToken cancellationToken);
    }
}
