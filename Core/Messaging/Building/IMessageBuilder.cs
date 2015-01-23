
namespace HackedBrain.ServiceBus.Core
{
    public interface IMessageBuilder
    {
        IMessage<T> BuildMessage<T>(T body);
    }
}
