using System.Collections.Generic;

namespace HackedBrain.ServiceBus.Core
{
    public interface IMessage
    {
        string Id
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

        object Body
        {
            get;
        }
    }
}
