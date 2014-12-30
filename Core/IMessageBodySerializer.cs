using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public interface IMessageBodySerializer
    {
        void SerializeBody<TBody>(TBody body, Stream destinationStream);

        object DeserializeBody(Stream sourceStream);
    }
}
