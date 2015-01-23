using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public class Dispatcher<T> : IDispatcher<T>
    {
        private Func<Type, IHandler> handlerFactory;
        private ConcurrentDictionary<Type, Func<T, Task>> handlerDispatchers = new ConcurrentDictionary<Type, Func<T, Task>>();

        public Dispatcher(Func<Type, IHandler> handlerFactory)
        {
            this.handlerFactory = handlerFactory;
        }

        public Task DispatchAsync(T what, CancellationToken cancellationToken)
        {
            Type whatType = what.GetType();

            Func<T, Task> handlerInvoker = this.GetHandlerInvoker(whatType);

            return handlerInvoker(what);
        }

        private Func<T, Task> GetHandlerInvoker(Type eventType)
        {
            return this.handlerDispatchers.GetOrAdd(
                eventType,
                ct =>
                {
                    // TODO: optimize this gross, non-performant reflection/dynamic logic by building dynamic lambda with expression API
                    return @event =>
                        {
                            IHandler handler = (IHandler)this.handlerFactory(ct);

                            return (Task)((dynamic)handler).Handle(@event);
                        };
                });
        }
    }
}
