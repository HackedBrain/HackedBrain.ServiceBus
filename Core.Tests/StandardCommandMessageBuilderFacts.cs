using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace HackedBrain.ServiceBus.Core.Tests
{
    public class StandardCommandMessageBuilderFacts
    {
        public class BuildMessageFacts : StandardCommandMessageBuilderFacts
        {
            [Fact]
            public void BuildMessageForNullThrows()
            {
                StandardCommandMessageBuilder commandMessageBuilder = new StandardCommandMessageBuilder(new Mock<IMessageMetadataProvider>().Object);
                
                Action buildMessage = () =>
                {
                    commandMessageBuilder.BuildMessage((ICommand)null);
                };

                buildMessage.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void BuildMessageReturnsMessageWithExpectedBody()
            {
                StandardCommandMessageBuilder commandMessageBuilder = new StandardCommandMessageBuilder(new Mock<IMessageMetadataProvider>().Object);

                TestCommand testCommand = new TestCommand();

                IMessage<TestCommand> message = commandMessageBuilder.BuildMessage(testCommand);

                message.Should().NotBeNull();

                message.Body.Should().BeSameAs(testCommand);
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
                messageMetadataProvider.Setup(mp => mp.GenerateMetadata(It.IsAny<TestCommand>()))
                    .Returns(testMetadata);
                
                StandardCommandMessageBuilder commandMessageBuilder = new StandardCommandMessageBuilder(messageMetadataProvider.Object);

                TestCommand testCommand = new TestCommand();
                
                IMessage<TestCommand> message = commandMessageBuilder.BuildMessage(testCommand);

                messageMetadataProvider.Verify(mp => mp.GenerateMetadata(
                    It.Is<TestCommand>(it => Object.ReferenceEquals(it, testCommand))),
                    Times.Once());

                message.Metadata.Should().BeSameAs(testMetadata);
            }
        }
    }
}
