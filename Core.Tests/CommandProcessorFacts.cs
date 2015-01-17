using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace HackedBrain.ServiceBus.Core.Tests
{
    public class CommandProcessorFacts
    {
        public class ConstructorFacts : CommandProcessorFacts
        {
            [Fact]
            public void ConstructWithNullMessageReceiverThrows()
            {
                Action constructor = () => new CommandProcessor(null, new Mock<ICommandDispatcher>().Object);

                constructor.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void ConstructWithNullCommandDispatcherThrows()
            {
                Action constructor = () => new CommandProcessor(new Mock<IMessageReceiver>().Object, null);

                constructor.ShouldThrow<ArgumentNullException>();
            }
        }

        public class StartFacts : CommandProcessorFacts
        {
            [Fact]
            public void StartShouldBeginListeningToMessageReceiver()
            {
                
            }
        }

        public class StopFacts : CommandProcessorFacts
        {
            [Fact]
            public void ShouldThrowExceptionIfNotAlreadyStarted()
            {
                CommandProcessor commandProcessor = new CommandProcessor(new Mock<IMessageReceiver>().Object, new Mock<ICommandDispatcher>().Object);

                Action stop = () => commandProcessor.Stop();

                stop.ShouldThrow<InvalidOperationException>();
            }

            [Fact]
            public void ShouldDisposeOfMessageReceiversSubscription()
            {
                Enumerable.Range(1, 5).Select(i => new TestCommand
                {
                    Id = i.ToString()
                });

                Mock<IMessageReceiver> mockMessageReceiver = new Mock<IMessageReceiver>();
                mockMessageReceiver.Setup(mr => mr.WhenMessageReceived<ICommand>())
                    .Returns(mockSubscriptionDisposable.Object);
                
                CommandProcessor commandProcessor = new CommandProcessor(mockMessageReceiver.Object, new Mock<ICommandDispatcher>().Object);

                commandProcessor.Stop();

                mockSubscriptionDisposable.Verify(d => d.Dispose(), Times.Once());
            }
        }
    }
}
