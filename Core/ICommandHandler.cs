using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<TCommand> : ICommandHandler
    {
        Task HandleAsync(TCommand command);
    }
}
