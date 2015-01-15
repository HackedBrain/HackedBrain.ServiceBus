using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace HackedBrain.ServiceBus.Core.Tests
{
    public class EventBusFacts
    {
        public class PublishEventAsyncFacts
        {
            [Fact]
            public void PublishingNullEventThrows()
            {
                EventBus eventBus = new EventBus(new Mock<IEventMessageBuilder>().Object, new Mock<IMessageSender>().Object);

                Func<Task> publishEventAsync = async () =>
                {
                    await eventBus.PublishEventAsync((IEvent)null, CancellationToken.None);
                };

                publishEventAsync.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public async Task PublishingEventBuildsMessage()
            {
                Mock<IEventMessageBuilder> mockMessageBuilder = new Mock<IEventMessageBuilder>();

                EventBus eventBus = new EventBus(mockMessageBuilder.Object, new Mock<IMessageSender>().Object);

                TestEvent testEvent = new TestEvent();
                
                await eventBus.PublishEventAsync(testEvent, CancellationToken.None);

                mockMessageBuilder.Verify(
                        mb => mb.BuildMessage<TestEvent>(It.Is<TestEvent>(it => Object.ReferenceEquals(it, testEvent))),
                        Times.Once());
            }

            [Fact]
            public async Task PublishingEventSendsViaMessageSender()
            {
                Mock<IMessage<TestEvent>> mockMessage = new Mock<IMessage<TestEvent>>();

                Mock<IEventMessageBuilder> mockMessageBuilder = new Mock<IEventMessageBuilder>();
                mockMessageBuilder.Setup(mb => mb.BuildMessage<TestEvent>(It.IsAny<TestEvent>()))
                    .Returns(mockMessage.Object);
                
                Mock<IMessageSender> mockMessageSender = new Mock<IMessageSender>();

                EventBus eventBus = new EventBus(mockMessageBuilder.Object, mockMessageSender.Object);

                TestEvent testEvent = new TestEvent();
                CancellationToken testCancellationToken = new CancellationToken();

                await eventBus.PublishEventAsync(testEvent, testCancellationToken);

                mockMessageSender.Verify(ms => 
                    ms.SendAsync(
                        It.Is<IMessage<TestEvent>>(m => Object.ReferenceEquals(m, mockMessage.Object)),
                        It.Is<CancellationToken>(ct => ct == testCancellationToken)),
                        Times.Once());
            }
        }

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
                private set;
            }
        }
    }
}
