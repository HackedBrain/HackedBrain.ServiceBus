using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace HackedBrain.ServiceBus.Core.Tests
{
    public class StandardEventMessageBuilderFacts
    {
        public class BuildMessageFacts : StandardEventMessageBuilderFacts
        {
            [Fact]
            public void BuildMessageForNullThrows()
            {
                StandardEventMessageBuilder eventMessageBuilder = new StandardEventMessageBuilder(new Mock<IMessageMetadataProvider>().Object);
                
                Action buildMessage = () =>
                {
                    eventMessageBuilder.BuildMessage((IEvent)null);
                };

                buildMessage.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void BuildMessageReturnsMessageWithExpectedBody()
            {
                StandardEventMessageBuilder eventMessageBuilder = new StandardEventMessageBuilder(new Mock<IMessageMetadataProvider>().Object);

                TestEvent testEvent = new TestEvent();

                IMessage<TestEvent> message = eventMessageBuilder.BuildMessage(testEvent);

                message.Should().NotBeNull();

                message.Body.Should().BeSameAs(testEvent);
            }

            [Fact]
            public void BuildMessageGeneratesMetadataAndAssignsToMessage()
            {
                IEnumerable<KeyValuePair<string, object>> testMetadata = new []
                { 
                    new KeyValuePair<string, object>("one", 1),
                    new KeyValuePair<string, object>("two", 2),
                };
                
                Mock<IMessageMetadataProvider> messageMetadataProvider = new Mock<IMessageMetadataProvider>();
                messageMetadataProvider.Setup(mp => mp.GenerateMetadata(It.IsAny<TestEvent>()))
                    .Returns(testMetadata);
                
                StandardEventMessageBuilder eventMessageBuilder = new StandardEventMessageBuilder(messageMetadataProvider.Object);

                TestEvent testEvent = new TestEvent();
                
                IMessage<TestEvent> message = eventMessageBuilder.BuildMessage(testEvent);

                messageMetadataProvider.Verify(mp => mp.GenerateMetadata(
                    It.Is<TestEvent>(it => Object.ReferenceEquals(it, testEvent))),
                    Times.Once());

                message.Metadata.Should().BeSameAs(testMetadata);
            }
        }
    }
}
