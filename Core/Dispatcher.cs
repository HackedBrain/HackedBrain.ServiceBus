using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public class Dispatcher : IDispatcher
    {
        private Func<Type, IHandler> handlerFactory;
        private ConcurrentDictionary<Type, Func<object, Task>> handlerDispatchers = new ConcurrentDictionary<Type, Func<object, Task>>();

        public Dispatcher(Func<Type, IHandler> handlerFactory)
        {
            this.handlerFactory = handlerFactory;
        }

        public Task DispatchAsync(object messageBody, CancellationToken cancellationToken)
        {
            Type messageBodyType = messageBody.GetType();

            Func<object, Task> handlerInvoker = this.GetHandlerInvoker(messageBodyType);

            return handlerInvoker(messageBodyType);
        }

        private Func<object, Task> GetHandlerInvoker(Type messageBodyType)
        {
            return this.handlerDispatchers.GetOrAdd(
                messageBodyType,
                ct =>
                {
                    // TODO: optimize this gross, non-performant dynamic logic by building strongly typed lambda with expression API
                    return messageBody =>
                        {
                            IHandler handler = this.handlerFactory(ct);

                            return ((dynamic)handler).HandleAsync(messageBody);
                        };
                });
        }
    }
}
