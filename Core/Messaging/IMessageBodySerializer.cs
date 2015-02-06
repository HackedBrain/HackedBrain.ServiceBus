using System.Collections.Generic;
using System.IO;

namespace HackedBrain.ServiceBus.Core
{
    public interface IMessageBodySerializer
    {
        IEnumerable<KeyValuePair<string, object>> SerializeBody<TBody>(TBody body, Stream destinationStream);

        object DeserializeBody(Stream sourceStream, IEnumerable<KeyValuePair<string, object>> metadata);
    }
}
