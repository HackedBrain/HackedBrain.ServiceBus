using System;

namespace HackedBrain.ServiceBus.Core.Tests
{
    public sealed class TestCommand : ICommand
    {
        public TestCommand()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }

        public string Id
        {
            get;
            private set;
        }
    }
}
