using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public interface IHandler
    {
    }

    public interface IHandler<T> : IHandler
    {
        Task HandleAsync(T what);
    }
}
