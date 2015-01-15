using System;
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
                CommandBus commandBus = new CommandBus(new Mock<ICommandMessageBuilder>().Object, new Mock<IMessageSender>().Object);

                Func<Task> sendCommandAsync = async () =>
                {
                    await commandBus.SendCommandAsync((ICommand)null, CancellationToken.None);
                };

                sendCommandAsync.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public async Task SendingCommandBuildsMessage()
            {
                Mock<ICommandMessageBuilder> mockMessageBuilder = new Mock<ICommandMessageBuilder>();

                CommandBus commandBus = new CommandBus(mockMessageBuilder.Object, new Mock<IMessageSender>().Object);

                TestCommand testCommand = new TestCommand();
                
                await commandBus.SendCommandAsync(testCommand, CancellationToken.None);
            }

            [Fact]
            public async Task SendingCommandSendsViaMessageSender()
            {
                Mock<IMessage<TestCommand>> mockMessage = new Mock<IMessage<TestCommand>>(); 
                
                Mock<ICommandMessageBuilder> mockMessageBuilder = new Mock<ICommandMessageBuilder>();
                mockMessageBuilder.Setup(mb => mb.BuildMessage<TestCommand>(It.IsAny<TestCommand>()))
                    .Returns(mockMessage.Object);
                
                Mock<IMessageSender> mockMessageSender = new Mock<IMessageSender>();

                CommandBus commandBus = new CommandBus(mockMessageBuilder.Object, mockMessageSender.Object);

                TestCommand testCommand = new TestCommand();
                CancellationToken testCancellationToken = new CancellationToken();

                await commandBus.SendCommandAsync(testCommand, testCancellationToken);

                mockMessageSender.Verify(ms => 
                    ms.SendAsync(
                        It.Is<IMessage<TestCommand>>(m => Object.ReferenceEquals(m, mockMessage.Object)), 
                        It.Is<CancellationToken>(ct => ct == testCancellationToken)),
                        Times.Once());
            }
        }

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
}
