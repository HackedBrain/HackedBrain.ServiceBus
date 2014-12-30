using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using HackedBrain.ServiceBus.Core;

namespace HackedBrain.ServiceBus.Serializers
{
    public class BinaryFormatterMessageBodySerializer : IMessageBodySerializer
    {
        #region IMessageBodySerializer implementation

        public void SerializeBody<TBody>(TBody body, Stream destinationStream)
        {
            BinaryFormatter binaryFormatter = BinaryFormatterMessageBodySerializer.BuildBinaryFormatter();

            binaryFormatter.Serialize(destinationStream, body);
        }

        public object DeserializeBody(Stream sourceStream)
        {
            BinaryFormatter binaryFormatter = BinaryFormatterMessageBodySerializer.BuildBinaryFormatter();

            return binaryFormatter.Deserialize(sourceStream);
        }

        #endregion

        #region Helper methods

        private static BinaryFormatter BuildBinaryFormatter()
        {
            BinaryFormatter result = new BinaryFormatter();
            result.AssemblyFormat = FormatterAssemblyStyle.Simple;

            return result;
        }

        #endregion
    }
}
