﻿using System;
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
                EventBus eventBus = new EventBus(new Mock<IMessageSender>().Object, new Mock<IMessageReceiver>().Object, new Mock<IMessageMetadataProvider>().Object);

                Func<Task> publishEventAsync = async () =>
                {
                    await eventBus.PublishEventAsync((object)null, CancellationToken.None);
                };

                publishEventAsync.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public async Task PublishingEventGeneratesMetadata()
            {
                Mock<IMessageMetadataProvider> mockMessageMetadataProvider = new Mock<IMessageMetadataProvider>();
                
                EventBus eventBus = new EventBus(new Mock<IMessageSender>().Object, new Mock<IMessageReceiver>().Object, mockMessageMetadataProvider.Object);

                TestEvent testEvent = new TestEvent();
                
                await eventBus.PublishEventAsync(testEvent, CancellationToken.None);

                mockMessageMetadataProvider.Verify(mmp => 
                    mmp.GenerateMetadata(
                        It.Is<TestEvent>(c => Object.ReferenceEquals(c, testEvent))),
                        Times.Once());
            }

            [Fact]
            public async Task PublishingEventSendsViaMessageSender()
            {
                Mock<IMessageSender> mockMessageSender = new Mock<IMessageSender>();

                Dictionary<string, object> testMetadata = new Dictionary<string,object>();

                Mock<IMessageMetadataProvider> mockMessageMetadataProvider = new Mock<IMessageMetadataProvider>();
                mockMessageMetadataProvider.Setup(mmp => mmp.GenerateMetadata<TestEvent>(It.IsAny<TestEvent>()))
                    .Returns(testMetadata);

                EventBus eventBus = new EventBus(mockMessageSender.Object, new Mock<IMessageReceiver>().Object, mockMessageMetadataProvider.Object);

                TestEvent testEvent = new TestEvent();
                CancellationToken testCancellationToken = new CancellationToken();

                await eventBus.PublishEventAsync(testEvent, testCancellationToken);

                mockMessageSender.Verify(ms => 
                    ms.SendAsync(
                        It.Is<TestEvent>(c => Object.ReferenceEquals(c, testEvent)), 
                        It.Is<IDictionary<string, object>>(d => Object.ReferenceEquals(d, testMetadata)),
                        It.Is<CancellationToken>(ct => ct == testCancellationToken)),
                        Times.Once());
            }
        }

        private sealed class TestEvent
        {
        
        }
    }
}