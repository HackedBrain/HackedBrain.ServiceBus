
namespace HackedBrain.ServiceBus.Core
{
    public interface IMessageBuilder
    {
        IMessage BuildMessage(object body);
    }
}
