using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace HackedBrain.ServiceBus.Core.Tests
{
    public class StandardMessageMetadataProviderFacts
    {
        public class SendCommandAsyncFacts
        {
            [Fact]
            public void GenerateMetadataForNullMessageThrows()
            {
                StandardMessageMetadataProvider standardMessageMetadataProvider = new StandardMessageMetadataProvider();
                
                Action generateMetadata = () =>
                {
                    standardMessageMetadataProvider.GenerateMetadata((object)null);
                };

                generateMetadata.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void GenerateMetadataPopulatesDictionaryWithExpectedKeyValues()
            {
                StandardMessageMetadataProvider standardMessageMetadataProvider = new StandardMessageMetadataProvider();

                IEnumerable<KeyValuePair<string, object>> metadata = standardMessageMetadataProvider.GenerateMetadata(new {});

                metadata.Should().NotBeNull();

                KeyValuePair<string, object> metadataEntry = metadata.FirstOrDefault(kvp => kvp.Key == StandardMessageMetadataProvider.ProviderVersionKey);
                metadataEntry.Value.As<string>().Should().NotBeNullOrWhiteSpace();

                metadataEntry = metadata.FirstOrDefault(kvp => kvp.Key == StandardMessageMetadataProvider.MessageTypeKey);
                metadataEntry.Value.As<string>().Should().NotBeNullOrWhiteSpace();

                metadataEntry = metadata.FirstOrDefault(kvp => kvp.Key == StandardMessageMetadataProvider.CreatedOnKey);
                metadataEntry.Value.As<DateTime>().Should().BeCloseTo(DateTime.UtcNow);
            }
        }
    }
}
