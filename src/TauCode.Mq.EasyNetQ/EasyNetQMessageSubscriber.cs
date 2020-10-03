using System;
using EasyNetQ;
using EasyNetQ.NonGeneric;
using TauCode.Mq.Exceptions;
using TauCode.Working;

namespace TauCode.Mq.EasyNetQ
{
    public class EasyNetQMessageSubscriber : MessageSubscriberBase, IEasyNetQMessageSubscriber
    {
        #region Fields

        private string _connectionString;
        private IBus _bus;

        #endregion

        #region Constructor

        public EasyNetQMessageSubscriber(IMessageHandlerContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        public EasyNetQMessageSubscriber(IMessageHandlerContextFactory contextFactory, string connectionString)
            : base(contextFactory)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region Overridden

        protected override void InitImpl()
        {
            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new MqException("Cannot start: connection string is null or empty.");
            }

            _bus = RabbitHutch.CreateBus(this.ConnectionString);
        }

        protected override void ShutdownImpl()
        {
            _bus.Dispose();
        }

        protected override IDisposable SubscribeImpl(ISubscriptionRequest subscriptionRequest)
        {
            if (subscriptionRequest.Handler != null)
            {
                // got sync handler
                var subscriptionId = Guid.NewGuid().ToString();

                IDisposable handle;

                if (subscriptionRequest.Topic == null)
                {
                    handle = _bus.Subscribe(
                        subscriptionRequest.MessageType,
                        subscriptionId,
                        subscriptionRequest.Handler);
                }
                else
                {
                    handle = _bus.Subscribe(
                        subscriptionRequest.MessageType,
                        subscriptionId,
                        subscriptionRequest.Handler,
                        configuration => configuration.WithTopic(subscriptionRequest.Topic));
                }

                return handle;
            }
            else
            {
                // got async handler
                var subscriptionId = Guid.NewGuid().ToString();

                IDisposable handle;

                if (subscriptionRequest.Topic == null)
                {
                    handle = _bus.SubscribeAsync(
                        subscriptionRequest.MessageType,
                        subscriptionId,
                        subscriptionRequest.AsyncHandler);
                }
                else
                {
                    handle = _bus.SubscribeAsync(
                        subscriptionRequest.MessageType,
                        subscriptionId,
                        subscriptionRequest.AsyncHandler,
                        configuration => configuration.WithTopic(subscriptionRequest.Topic));
                }

                return handle;
            }
        }

        #endregion

        #region IEasyNetQMessageSubscriber Members

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                if (this.State != WorkerState.Stopped)
                {
                    throw new MqException("Cannot set connection string while subscriber is running.");
                }

                if (this.IsDisposed)
                {
                    throw new ObjectDisposedException(this.Name);
                }

                _connectionString = value;
            }
        }

        #endregion
    }
}
