
namespace HackedBrain.ServiceBus.Core
{
    public interface IMessageSessionIdProvider
    {
        string SessionId
        {
            get;
        }
    }
}
