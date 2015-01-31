using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;

namespace HackedBrain.ServiceBus.Core.Tests
{
    public class CompositeMessageMetadataProviderFacts
    {
        public class ConstructorFacts : CompositeMessageMetadataProviderFacts
        {
            [Fact]
            public void ConstructingWithANullSetOfProvidersThrows()
            {
                Action constructInstance = () =>
                {
                    CompositeMessageMetadataProvider compositeMessageMetadataProvider = new CompositeMessageMetadataProvider(null);
                };

                constructInstance.ShouldThrow<ArgumentNullException>();
            }
        }

        public class GenerateMetadataFacts : CompositeMessageMetadataProviderFacts
        {
            [Fact]
            public void GenerateMetadataWithNullMessageThrows()
            {
                CompositeMessageMetadataProvider compositeMessageMetadataProvider = new CompositeMessageMetadataProvider(new[]
                {
                    new Mock<IMessageMetadataProvider>().Object
                });

                Action generateMetadata = () =>
                {
                    compositeMessageMetadataProvider.GenerateMetadata((object)null);
                };

                generateMetadata.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void GenerateMetadataShouldCallGenerateMetadataOnAllChildProviders()
            {
                Dictionary<int, Mock<IMessageMetadataProvider>> mockMessageMetadataProviders = new Dictionary<int, Mock<IMessageMetadataProvider>>();
                
                foreach(int metadataProviderId in Enumerable.Range(1, 5))
                {
                    Mock<IMessageMetadataProvider> mockMessageMetadataProvider = new Mock<IMessageMetadataProvider>();
                    
                    mockMessageMetadataProvider.Setup(mmp => mmp.GenerateMetadata(It.IsAny<TestMessage>()))
                        .Returns(() => new[] { new KeyValuePair<string, object>(metadataProviderId.ToString(), metadataProviderId) });

                    mockMessageMetadataProviders.Add(metadataProviderId, mockMessageMetadataProvider);
                }

                CompositeMessageMetadataProvider compositeMessageMetadataProvider = new CompositeMessageMetadataProvider(mockMessageMetadataProviders.Values.Select(mmmp => mmmp.Object));

                TestMessage testMessage = new TestMessage();

                List<KeyValuePair<string, object>> metadata = compositeMessageMetadataProvider.GenerateMetadata(testMessage).ToList();
                
                // Make sure each child metadata provider was invoked as expected
                foreach(Mock<IMessageMetadataProvider> mockMessageMetadataProvider in mockMessageMetadataProviders.Values)
                {
                    mockMessageMetadataProvider.Verify(mmp => mmp.GenerateMetadata(It.Is<TestMessage>(it => Object.ReferenceEquals(it, testMessage))), Times.Once());
                }

                // Make sure each child metadata provider's data was returned (and no extra data was returned)
                metadata.Count.Should().Be(mockMessageMetadataProviders.Count);
                metadata.Should().BeEquivalentTo(mockMessageMetadataProviders.Keys.Select(k => new KeyValuePair<string, object>(k.ToString(), k)));
            }

            private class TestMessage
            {

            }
        }
    }
}
