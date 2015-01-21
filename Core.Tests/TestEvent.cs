using System;

namespace HackedBrain.ServiceBus.Core.Tests
{
    public sealed class TestEvent : IEvent
    {
        public TestEvent()
        {
            this.SourceId = Guid.NewGuid().ToString("N");
        }

        public TestEvent(string sourceId)
        {
            this.SourceId = sourceId;
        }

        public string SourceId
        {
            get;
            set;
        }
    }
}
