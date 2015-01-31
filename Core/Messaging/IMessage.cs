using System.Collections.Generic;

namespace HackedBrain.ServiceBus.Core
{
    public interface IMessage<TBody>
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

        TBody Body
        {
            get;
        }
    }
}
