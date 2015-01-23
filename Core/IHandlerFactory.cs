
namespace HackedBrain.ServiceBus.Core
{
    public interface IHandlerFactory
    {
        IHandler<T> CreateHandler<T>();
    }
}
