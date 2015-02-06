using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public interface IMessageSender
    {
        Task SendAsync(IMessage message, CancellationToken cancellationToken);
    }
}
