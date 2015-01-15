using System;
using System.Collections.Generic;
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
                EventBus eventBus = new EventBus(new Mock<IEventMessageBuilder>().Object, new Mock<IMessageSender>().Object, new Mock<IMessageMetadataProvider>().Object);

                Func<Task> publishEventAsync = async () =>
                {
                    await eventBus.PublishEventAsync((IEvent)null, CancellationToken.None);
                };

                publishEventAsync.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public async Task PublishingEventGeneratesMetadata()
            {
                Mock<IMessageMetadataProvider> mockMessageMetadataProvider = new Mock<IMessageMetadataProvider>();

                EventBus eventBus = new EventBus(new Mock<IEventMessageBuilder>().Object, new Mock<IMessageSender>().Object, mockMessageMetadataProvider.Object);

                TestEvent testEvent = new TestEvent();
                
                await eventBus.PublishEventAsync(testEvent, CancellationToken.None);

                mockMessageMetadataProvider.Verify(mmp => 
                    mmp.GenerateMetadata(
                        It.Is<IEvent>(c => Object.ReferenceEquals(c, testEvent))),
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

                Dictionary<string, object> testMetadata = new Dictionary<string,object>();

                Mock<IMessageMetadataProvider> mockMessageMetadataProvider = new Mock<IMessageMetadataProvider>();
                mockMessageMetadataProvider.Setup(mmp => mmp.GenerateMetadata<TestEvent>(It.IsAny<TestEvent>()))
                    .Returns(testMetadata);

                EventBus eventBus = new EventBus(mockMessageBuilder.Object, mockMessageSender.Object, mockMessageMetadataProvider.Object);

                TestEvent testEvent = new TestEvent();
                CancellationToken testCancellationToken = new CancellationToken();

                await eventBus.PublishEventAsync(testEvent, testCancellationToken);

                mockMessageSender.Verify(ms => 
                    ms.SendAsync(
                        It.Is<IMessage<TestEvent>>(m => Object.ReferenceEquals(m.Body, testEvent)),
                        It.Is<CancellationToken>(ct => ct == testCancellationToken)),
                        Times.Once());
            }
        }

        private sealed class TestEvent : IEvent
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
