using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HackedBrain.ServiceBus.Core
{
    public class CommandProcessor : IProcessor, IDisposable
    {
        #region Fields

        private ICommandDispatcher commandDispatcher;
        private IMessageReceiver messageReceiver;
        private IDisposable messageDisatchingSubscription;
        private Subject<ICommand> commandProcessedSubject;

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
            this.commandProcessedSubject = new Subject<ICommand>();
        }

        #endregion

        #region Type specific methods

        public IObservable<ICommand> WhenCommandProcessed()
        {
            return this.commandProcessedSubject;
        }

        #endregion

        #region IProcessor implementation

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
                .Subscribe(async message => 
                    {
                        ICommand command = message.Body;
                        
                        await this.commandDispatcher.DispatchAsync(command, commandDispatchingDisposable.Token);

                        this.commandProcessedSubject.OnNext(command);
                    });

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

            this.commandProcessedSubject.OnCompleted();
            this.commandProcessedSubject.Dispose();
            this.commandProcessedSubject = new Subject<ICommand>();
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            if(this.messageDisatchingSubscription != null)
            {
                this.Stop();
            }
        }

        #endregion
    }
}
