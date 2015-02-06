using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace HackedBrain.ServiceBus.Core.Tests
{
    public class StandardMessageBuilderFacts
    {
        public class BuildMessageFacts : StandardMessageBuilderFacts
        {
            [Fact]
            public void BuildMessageForNullThrows()
            {
                StandardMessageBuilder messageBuilder = new StandardMessageBuilder(t => null, t => null, t => null, t => null);
                
                Action buildMessage = () =>
                {
                    messageBuilder.BuildMessage((TestEvent)null);
                };

                buildMessage.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void BuildMessageReturnsMessageWithExpectedBody()
            {
                StandardMessageBuilder messageBuilder = new StandardMessageBuilder(t => null, t => null, t => null, t => null);

                TestEvent testEvent = new TestEvent();

                IMessage message = messageBuilder.BuildMessage(testEvent);

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
                messageMetadataProvider.Setup(mp => mp.GenerateMetadata(It.IsAny<object>()))
                    .Returns(() => testMetadata);

                StandardMessageBuilder messageBuilder = new StandardMessageBuilder(t => null, t => null, t => null, t => messageMetadataProvider.Object);

                TestEvent testEvent = new TestEvent();
                
                IMessage message = messageBuilder.BuildMessage(testEvent);

                messageMetadataProvider.Verify(mp => mp.GenerateMetadata(
                    It.Is<object>(it => Object.ReferenceEquals(it, testEvent))),
                    Times.Once());

                message.Metadata.Should().BeEquivalentTo(testMetadata);
            }
        }
    }
}
