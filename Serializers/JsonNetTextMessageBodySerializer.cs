﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using Newtonsoft.Json;

namespace HackedBrain.ServiceBus.Serializers
{
    public class JsonNetTextMessageBodySerializer : IMessageBodySerializer
    {
        #region Fields

        public static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
            TypeNameHandling = TypeNameHandling.All
        };

        private JsonSerializer jsonSerializer;

        #endregion

        #region Constructors

        public JsonNetTextMessageBodySerializer() : this(JsonNetTextMessageBodySerializer.DefaultJsonSerializerSettings)
        {
        }

        public JsonNetTextMessageBodySerializer(JsonSerializerSettings serializerSettings)
        {
            this.jsonSerializer = JsonSerializer.Create(serializerSettings);
        }

        #endregion

        #region IMessageBodySerializer implementation

        public void SerializeBody<TBody>(TBody body, Stream destinationStream)
        {
            using(StreamWriter streamWriter = new StreamWriter(destinationStream))
            using(JsonTextWriter jsonWriter = new JsonTextWriter(streamWriter))
            {
                this.jsonSerializer.Serialize(jsonWriter, body);
            }
        }

        public TBody DeserializeBody<TBody>(Stream sourceStream)
        {
            using(StreamReader streamReader = new StreamReader(sourceStream))
            using(JsonTextReader jsonReader = new JsonTextReader(streamReader))
            {
                return this.jsonSerializer.Deserialize<TBody>(jsonReader);
            }
        }

        #endregion
    }
}