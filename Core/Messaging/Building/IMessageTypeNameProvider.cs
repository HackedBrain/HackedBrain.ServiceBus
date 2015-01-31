
namespace HackedBrain.ServiceBus.Core
{
    public interface IMessageTypeNameProvider
    {
        string GenerateMessageTypeName<T>(T body);
    }
}
