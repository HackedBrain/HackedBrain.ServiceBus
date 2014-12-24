using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus
{
	public interface ICommandBus
	{
		Task SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : class;
	}
}
