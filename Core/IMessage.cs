using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
	public interface IMessage
	{
		IEnumerable<KeyValuePair<string, object>> Metadata
		{
			get;
		}

		T GetBody<T>() where T : class;

		void Complete();

		Task CompleteAsync();

		void Abandon();

		Task AbandonAsync();

		void Quarantine(string reason, string description);

		Task QuarantineAsync(string reason, string description);
	}
}
