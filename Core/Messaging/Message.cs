using System.Collections.Generic;

namespace HackedBrain.ServiceBus.Core
{
    public class Message : IMessage
    {
        private object body;

        public Message(object body)
        {
            this.body = body;
        }

        public string Id
        {
            get;
            set;
        }

        public string SessionId
        {
            get;
            set;
        }

        public string CorrelationId
        {
            get;
            set;
        }

        public IEnumerable<KeyValuePair<string, object>> Metadata
        {
            get;
            set;
        }

        public object Body
        {
            get
            {
                return this.body;
            }
        }
    }
}
