using EasyNetQ;
using EasyNetQ.NonGeneric;
using System;

namespace TauCode.Mq.EasyNetQ
{
    public class EasyNetQMessageSubscriber : MessageSubscriberBase
    {
        #region Fields

        private readonly string _connectionString;
        private IBus _bus;

        #endregion

        #region Constructor

        public EasyNetQMessageSubscriber(string name, string connectionString)
            : base(name)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        #endregion

        #region Overridden

        protected override IDisposable SubscribeImpl(Type messageType, string subscriptionId, Action<object> callback)
        {
            return _bus.Subscribe(messageType, subscriptionId, callback);
        }

        protected override void StartImpl()
        {
            _bus = RabbitHutch.CreateBus(_connectionString);
        }

        protected override void DisposeImpl()
        {
            _bus.Dispose();
        }

        #endregion
    }
}
