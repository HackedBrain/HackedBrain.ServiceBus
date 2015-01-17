
namespace HackedBrain.ServiceBus.Core.Tests
{
    public sealed class TestCommand : ICommand
    {
        public TestCommand()
        {
        }

        public string Id
        {
            get;
            set;
        }
    }
}
