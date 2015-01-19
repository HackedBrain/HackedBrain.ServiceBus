using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Collections.Generic;

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
            public async Task ShouldProcessMessagesFromReceiver()
            {
                Subject<IMessage<ICommand>> testCommandMessagesSubject = new Subject<IMessage<ICommand>>();

                Mock<IMessageReceiver> mockMessageReceiver = new Mock<IMessageReceiver>();
                mockMessageReceiver.Setup(mr => mr.WhenMessageReceived<ICommand>(default(TimeSpan)))
                    .Returns(testCommandMessagesSubject);

                TestCommand testCommand = new TestCommand();
                
                Mock<IMessage<ICommand>> mockCommandMessage = new Mock<IMessage<ICommand>>();
                mockCommandMessage.SetupGet(m => m.Body)
                    .Returns(testCommand);

                using(CommandProcessor commandProcessor = new CommandProcessor(mockMessageReceiver.Object, new Mock<ICommandDispatcher>().Object))
                {
                    IConnectableObservable<ICommand> processedCommands = commandProcessor.WhenCommandProcessed().ObserveOn(TaskPoolScheduler.Default).Replay();

                    using(processedCommands.Connect())
                    {
                        commandProcessor.Start();

                        testCommandMessagesSubject.HasObservers.Should().BeTrue();

                        testCommandMessagesSubject.OnNext(mockCommandMessage.Object);

                        ICommand command = await processedCommands.FirstOrDefaultAsync();

                        command.Should().BeSameAs(testCommand);
                    }
                }

                mockMessageReceiver.Verify(mr => mr.WhenMessageReceived<ICommand>(It.IsAny<TimeSpan>()), Times.Once());
            }

            [Fact]
            public async Task ShouldStopReceivingMessagesWhenStopped()
            {
                Subject<IMessage<ICommand>> testCommandMessagesSubject = new Subject<IMessage<ICommand>>();

                Mock<IMessageReceiver> mockMessageReceiver = new Mock<IMessageReceiver>();
                mockMessageReceiver.Setup(mr => mr.WhenMessageReceived<ICommand>(default(TimeSpan)))
                    .Returns(testCommandMessagesSubject);

                TestCommand testCommand = new TestCommand();
                
                Mock<IMessage<ICommand>> mockCommandMessage = new Mock<IMessage<ICommand>>();
                mockCommandMessage.SetupGet(m => m.Body)
                    .Returns(testCommand);

                using(CommandProcessor commandProcessor = new CommandProcessor(mockMessageReceiver.Object, new Mock<ICommandDispatcher>().Object))
                {
                    IConnectableObservable<ICommand> processedCommands = commandProcessor.WhenCommandProcessed().ObserveOn(TaskPoolScheduler.Default).Publish();

                    using(processedCommands.Connect())
                    {
                        commandProcessor.Start();

                        testCommandMessagesSubject.HasObservers.Should().BeTrue();

                        testCommandMessagesSubject.OnNext(mockCommandMessage.Object);

                        commandProcessor.Stop();

                        testCommandMessagesSubject.HasObservers.Should().BeFalse();

                        testCommandMessagesSubject.OnNext(mockCommandMessage.Object);

                        ICommand command = await processedCommands.LastOrDefaultAsync();

                        command.Should().BeNull();
                    }
                }
            }

            [Fact]
            public async Task ShouldDeliverCommandsToDispatcher()
            {
                Subject<IMessage<ICommand>> testCommandMessagesSubject = new Subject<IMessage<ICommand>>();

                Mock<IMessageReceiver> mockMessageReceiver = new Mock<IMessageReceiver>();
                mockMessageReceiver.Setup(mr => mr.WhenMessageReceived<ICommand>(default(TimeSpan)))
                    .Returns(testCommandMessagesSubject);

                Mock<ICommandDispatcher> mockCommandDispatcher = new Mock<ICommandDispatcher>();

                List<IMessage<ICommand>> testCommandMessages = Enumerable.Range(1, 3).Select(i => 
                {
                    string id = i.ToString();
                    
                    Mock<IMessage<ICommand>> mockCommandMessage = new Mock<IMessage<ICommand>>();
                    mockCommandMessage.SetupGet(m => m.Id)
                        .Returns("Message:" + id);

                    mockCommandMessage.SetupGet(m => m.Body)
                        .Returns(new TestCommand
                        {
                            Id = "Command:" + id
                        });

                    return mockCommandMessage.Object;
                }).ToList();
                
                using(CommandProcessor commandProcessor = new CommandProcessor(mockMessageReceiver.Object, mockCommandDispatcher.Object))
                {
                    commandProcessor.Start();

                    foreach(IMessage<ICommand> commandMessage in testCommandMessages)
                    {
                        testCommandMessagesSubject.OnNext(commandMessage);
                    }
                }

                mockCommandDispatcher.Verify(
                    cd => cd.DispatchAsync(
                        It.IsAny<ICommand>(), 
                        It.IsAny<CancellationToken>()),
                    Times.Exactly(testCommandMessages.Count));

                throw new NotImplementedException("Should really verify the commands are delivered in order!");
            }
        }
    }
}
