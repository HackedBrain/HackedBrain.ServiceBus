using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace HackedBrain.ServiceBus.Core.Tests
{
    public class MessageProcessorFacts
    {
        public class ConstructorFacts : MessageProcessorFacts
        {
            [Fact]
            public void ConstructWithNullMessageReceiverThrows()
            {
                Action constructor = () => new MessageProcessor(null, new Mock<IDispatcher>().Object);

                constructor.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void ConstructWithNullEventDispatcherThrows()
            {
                Action constructor = () => new MessageProcessor(new Mock<IMessageReceiver>().Object, null);

                constructor.ShouldThrow<ArgumentNullException>();
            }
        }

        public class StartFacts : MessageProcessorFacts
        {
            [Fact]
            public void ShouldThrowExceptionIfAlreadyStarted()
            {
                Subject<IMessage> testMessagesSubject = new Subject<IMessage>();

                Mock<IMessageReceiver> mockMessageReceiver = new Mock<IMessageReceiver>();
                mockMessageReceiver.Setup(mr => mr.WhenMessageReceived(default(TimeSpan)))
                    .Returns(testMessagesSubject);
                
                MessageProcessor messageProcessor = new MessageProcessor(mockMessageReceiver.Object, new Mock<IDispatcher>().Object);

                messageProcessor.Start();

                Action startAgain = () => messageProcessor.Start();

                startAgain.ShouldThrow<InvalidOperationException>();
            }
            
            [Fact]
            public async Task ShouldProcessMessagesFromReceiver()
            {
                Subject<IMessage> testMessagesSubject = new Subject<IMessage>();

                Mock<IMessageReceiver> mockMessageReceiver = new Mock<IMessageReceiver>();
                mockMessageReceiver.Setup(mr => mr.WhenMessageReceived(default(TimeSpan)))
                    .Returns(testMessagesSubject);

                Mock<IMessage> mockMessage = new Mock<IMessage>();
                mockMessage.SetupGet(m => m.Body)
                    .Returns(new object());

                using(MessageProcessor messageProcessor = new MessageProcessor(mockMessageReceiver.Object, new Mock<IDispatcher>().Object))
                {
                    IConnectableObservable<IMessage> processedMessages = messageProcessor.WhenMessageProcessed().ObserveOn(TaskPoolScheduler.Default).Replay();

                    using(processedMessages.Connect())
                    {
                        messageProcessor.Start();

                        testMessagesSubject.HasObservers.Should().BeTrue();

                        testMessagesSubject.OnNext(mockMessage.Object);

                        IMessage message = await processedMessages.FirstOrDefaultAsync();

                        message.Should().BeSameAs(mockMessage.Object);
                    }
                }

                mockMessageReceiver.Verify(mr => mr.WhenMessageReceived(It.IsAny<TimeSpan>()), Times.Once());
            }
        }

        public class MessageDeliveryFacts : MessageProcessorFacts
        {
            [Fact]
            public void ShouldDeliverMessagesToDispatcher()
            {
                Subject<IMessage> testMessagesSubject = new Subject<IMessage>();

                Mock<IMessageReceiver> mockMessageReceiver = new Mock<IMessageReceiver>();
                mockMessageReceiver.Setup(mr => mr.WhenMessageReceived(default(TimeSpan)))
                    .Returns(testMessagesSubject);

                Mock<IDispatcher> mockEventDispatcher = new Mock<IDispatcher>();

                List<IMessage> testMessages = Enumerable.Range(1, 3).Select(i =>
                {
                    string id = i.ToString();

                    Mock<IMessage> mockEventMessage = new Mock<IMessage>();
                    mockEventMessage.SetupGet(m => m.Id)
                        .Returns("Message:" + id);

                    mockEventMessage.SetupGet(m => m.Body)
                        .Returns(new object());

                    return mockEventMessage.Object;
                }).ToList();

                MockSequence mockSequence = new MockSequence();

                using(MessageProcessor messageProcessor = new MessageProcessor(mockMessageReceiver.Object, mockEventDispatcher.Object))
                {
                    messageProcessor.Start();

                    foreach(IMessage eventMessage in testMessages)
                    {
                        testMessagesSubject.OnNext(eventMessage);
                    }
                }

                mockEventDispatcher.Verify(
                    cd => cd.DispatchAsync(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()),
                    Times.Exactly(testMessages.Count));

                // TODO: consider verifying the exact sequence of delivery, can't figure out "good" way to do this with Moq right now
            }
        }

        public class StopFacts : MessageProcessorFacts
        {
            [Fact]
            public void ShouldThrowExceptionIfNotAlreadyStarted()
            {
                MessageProcessor messageProcessor = new MessageProcessor(new Mock<IMessageReceiver>().Object, new Mock<IDispatcher>().Object);

                Action stop = () => messageProcessor.Stop();

                stop.ShouldThrow<InvalidOperationException>();
            }
            

            [Fact]
            public async Task ShouldStopReceivingMessagesWhenStopped()
            {
                Subject<IMessage> testMessagesSubject = new Subject<IMessage>();

                Mock<IMessageReceiver> mockMessageReceiver = new Mock<IMessageReceiver>();
                mockMessageReceiver.Setup(mr => mr.WhenMessageReceived(default(TimeSpan)))
                    .Returns(testMessagesSubject);

                Mock<IMessage> mockMessage = new Mock<IMessage>();
                mockMessage.SetupGet(m => m.Body)
                    .Returns(new object());

                using(MessageProcessor messageProcessor = new MessageProcessor(mockMessageReceiver.Object, new Mock<IDispatcher>().Object))
                {
                    IConnectableObservable<object> processedEvents = messageProcessor.WhenMessageProcessed().ObserveOn(TaskPoolScheduler.Default).Select(m => m.Body).Publish();

                    using(processedEvents.Connect())
                    {
                        messageProcessor.Start();

                        testMessagesSubject.HasObservers.Should().BeTrue();

                        testMessagesSubject.OnNext(mockMessage.Object);

                        messageProcessor.Stop();

                        testMessagesSubject.HasObservers.Should().BeFalse();

                        testMessagesSubject.OnNext(mockMessage.Object);

                        object message = await processedEvents.LastOrDefaultAsync();

                        message.Should().BeNull();
                    }
                }
            }
        }
    }
}
