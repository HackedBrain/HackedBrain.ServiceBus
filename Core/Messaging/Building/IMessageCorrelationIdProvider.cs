
namespace HackedBrain.ServiceBus.Core.Messaging
{
    public interface IMessageCorrelationIdProvider
    {
        string GenerateCorrelationId<T>(T body);
    }
}
