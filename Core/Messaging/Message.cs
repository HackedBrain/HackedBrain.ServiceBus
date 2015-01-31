using System.Collections.Generic;

namespace HackedBrain.ServiceBus.Core
{
    public class Message<TBody> : IMessage<TBody>
    {
        private TBody body;

        public Message(TBody body)
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

        public TBody Body
        {
            get
            {
                return this.body;
            }
        }
    }
}
