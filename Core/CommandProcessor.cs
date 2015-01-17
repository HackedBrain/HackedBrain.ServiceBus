using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace HackedBrain.ServiceBus.Core
{
    public class CommandProcessor : IProcessor
    {
        #region Fields

        private ICommandDispatcher commandDispatcher;
        private IMessageReceiver messageReceiver;
        private IDisposable messageDisatchingSubscription;

        #endregion

        #region Constructors

        public CommandProcessor(IMessageReceiver messageReceiver, ICommandDispatcher commandDispatcher)
        {
            if(messageReceiver == null)
            {
                throw new ArgumentNullException("messageReceiver");
            }

            this.messageReceiver = messageReceiver;
            
            if(commandDispatcher == null)
            {
                throw new ArgumentNullException("commandDispatcher");
            }
            
            this.commandDispatcher = commandDispatcher;
        }

        #endregion

        #region Type specific methods

        public void Start()
        {
            if(this.messageDisatchingSubscription != null)
            {
                throw new InvalidOperationException("The processor is already started.");
            }

            CancellationDisposable commandDispatchingDisposable = new CancellationDisposable();

            IDisposable commandProcessingDisposable = this.messageReceiver.WhenMessageReceived<ICommand>()
#if DEBUG                
                .Do(message => Debug.WriteLine("Processing command: type={0};id={1};", message.Body.GetType().Name, message.Body.Id))
#endif
                .Subscribe(async message => await this.commandDispatcher.DispatchAsync(message.Body, commandDispatchingDisposable.Token));

            this.messageDisatchingSubscription = new CompositeDisposable(commandProcessingDisposable, commandDispatchingDisposable);
        }

        public void Stop()
        {
            if(this.messageDisatchingSubscription == null)
            {
                throw new InvalidOperationException("The processor has not been started.");
            }

            this.messageDisatchingSubscription.Dispose();
            this.messageDisatchingSubscription = null;
        }

        #endregion
    }
}
