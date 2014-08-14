using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
	public interface IMessage
	{
		IDictionary<string, object> Metadata
		{
			get;
		}

		T GetBody<T>() where T : class;

		void Complete();

		Task CompleteAsync();

		void Abandon();

		Task AbandonAsync();
	}
}
