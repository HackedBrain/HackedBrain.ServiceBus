
namespace HackedBrain.ServiceBus.Core
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> CreateHandler<TCommand>();
    }
}
