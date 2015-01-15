using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public interface IMessage
    {
        Guid Id
        {
            get;
        }

        string SessionId
        {
            get;
        }

        string CorrelationId
        {
            get;
        }

        IEnumerable<KeyValuePair<string, object>> Metadata
        {
            get;
        }

        T GetBody<T>() where T : class;

        Task CompleteAsync();

        Task AbandonAsync();

        Task QuarantineAsync(string reason, string description);
    }
}
