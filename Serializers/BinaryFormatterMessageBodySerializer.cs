using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using HackedBrain.ServiceBus.Core;

namespace HackedBrain.ServiceBus.Serializers
{
    public class BinaryFormatterMessageBodySerializer : IMessageBodySerializer
    {
        private static readonly IEnumerable<KeyValuePair<string, object>> EmptyMetadata = Enumerable.Empty<KeyValuePair<string, object>>();
        
        #region IMessageBodySerializer implementation

        public IEnumerable<KeyValuePair<string, object>> SerializeBody<TBody>(TBody body, Stream destinationStream)
        {
            BinaryFormatter binaryFormatter = BinaryFormatterMessageBodySerializer.BuildBinaryFormatter();

            binaryFormatter.Serialize(destinationStream, body);

            return BinaryFormatterMessageBodySerializer.EmptyMetadata;
        }

        public TBody DeserializeBody<TBody>(Stream sourceStream, IEnumerable<KeyValuePair<string, object>> metadata)
        {
            BinaryFormatter binaryFormatter = BinaryFormatterMessageBodySerializer.BuildBinaryFormatter();

            return (TBody)binaryFormatter.Deserialize(sourceStream);
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
