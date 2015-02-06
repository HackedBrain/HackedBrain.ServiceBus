﻿using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HackedBrain.ServiceBus.Core
{
    public class MessageProcessor : IProcessor, IDisposable
    {
        #region Fields

        private IDispatcher dispatcher;
        private IMessageReceiver messageReceiver;
        private IDisposable messageDisatchingSubscription;
        private Subject<IMessage> messageProcessedSubject;

        #endregion

        #region Constructors

        public MessageProcessor(IMessageReceiver messageReceiver, IDispatcher dispatcher)
        {
            if(messageReceiver == null)
            {
                throw new ArgumentNullException("messageReceiver");
            }

            this.messageReceiver = messageReceiver;
            
            if(dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }
            
            this.dispatcher = dispatcher;
            this.messageProcessedSubject = new Subject<IMessage>();
        }

        #endregion

        #region Type specific methods

        public IObservable<IMessage> WhenMessageProcessed()
        {
            return this.messageProcessedSubject;
        }

        #endregion

        #region IProcessor implementation

        public void Start()
        {
            if(this.messageDisatchingSubscription != null)
            {
                throw new InvalidOperationException("The processor is already started.");
            }

            CancellationDisposable messageDispatchingDisposable = new CancellationDisposable();

            IDisposable messageProcessingDisposable = this.messageReceiver.WhenMessageReceived()
#if DEBUG                
                .Do(message => Debug.WriteLine("Processing event: BodyType={0};", message.Body.GetType().Name))
#endif
                .Subscribe(async message => 
                    {
                        object messageBody = message.Body;
                        
                        await this.dispatcher.DispatchAsync(messageBody, messageDispatchingDisposable.Token);

                        this.messageProcessedSubject.OnNext(message);
                    });

            this.messageDisatchingSubscription = new CompositeDisposable(messageProcessingDisposable, messageDispatchingDisposable);
        }

        public void Stop()
        {
            if(this.messageDisatchingSubscription == null)
            {
                throw new InvalidOperationException("The processor has not been started.");
            }

            this.messageDisatchingSubscription.Dispose();
            this.messageDisatchingSubscription = null;

            this.messageProcessedSubject.OnCompleted();
            this.messageProcessedSubject.Dispose();
            this.messageProcessedSubject = new Subject<IMessage>();
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
