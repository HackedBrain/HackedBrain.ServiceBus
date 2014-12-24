using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace HackedBrain.ServiceBus.Core.Tests
{
    public class CommandBusFacts
    {
        public class SendCommandAsyncFacts
        {
            [Fact]
            public void SendingNullCommandThrows()
            {
                CommandBus commandBus = new CommandBus(new Mock<IMessageSender>().Object, new Mock<IMessageReceiver>().Object, new Mock<IMessageMetadataProvider>().Object);

                Func<Task> sendCommandAsync = async () =>
                {
                    await commandBus.SendCommandAsync((object)null, CancellationToken.None);
                };

                sendCommandAsync.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public async Task SendingCommandGeneratesMetadata()
            {
                Mock<IMessageMetadataProvider> mockMessageMetadataProvider = new Mock<IMessageMetadataProvider>();
                
                CommandBus commandBus = new CommandBus(new Mock<IMessageSender>().Object, new Mock<IMessageReceiver>().Object, mockMessageMetadataProvider.Object);

                TestCommand testCommand = new TestCommand();
                
                await commandBus.SendCommandAsync(testCommand, CancellationToken.None);

                mockMessageMetadataProvider.Verify(mmp => 
                    mmp.GenerateMetadata(
                        It.Is<TestCommand>(c => Object.ReferenceEquals(c, testCommand))),
                        Times.Once());
            }

            [Fact]
            public async Task SendingCommandSendsViaMessageSender()
            {
                Mock<IMessageSender> mockMessageSender = new Mock<IMessageSender>();

                Dictionary<string, object> testMetadata = new Dictionary<string,object>();

                Mock<IMessageMetadataProvider> mockMessageMetadataProvider = new Mock<IMessageMetadataProvider>();
                mockMessageMetadataProvider.Setup(mmp => mmp.GenerateMetadata<TestCommand>(It.IsAny<TestCommand>()))
                    .Returns(testMetadata);

                CommandBus commandBus = new CommandBus(mockMessageSender.Object, new Mock<IMessageReceiver>().Object, mockMessageMetadataProvider.Object);

                TestCommand testCommand = new TestCommand();
                CancellationToken testCancellationToken = new CancellationToken();

                await commandBus.SendCommandAsync(testCommand, testCancellationToken);

                mockMessageSender.Verify(ms => 
                    ms.SendAsync(
                        It.Is<TestCommand>(c => Object.ReferenceEquals(c, testCommand)), 
                        It.Is<IDictionary<string, object>>(d => Object.ReferenceEquals(d, testMetadata)),
                        It.Is<CancellationToken>(ct => ct == testCancellationToken)),
                        Times.Once());
            }
        }

        private sealed class TestCommand
        {
        
        }
    }
}
