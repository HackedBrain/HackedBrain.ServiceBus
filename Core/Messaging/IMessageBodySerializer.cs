using System.IO;

namespace HackedBrain.ServiceBus.Core
{
    public interface IMessageBodySerializer
    {
        void SerializeBody<TBody>(TBody body, Stream destinationStream);

        TBody DeserializeBody<TBody>(Stream sourceStream);
    }
}
