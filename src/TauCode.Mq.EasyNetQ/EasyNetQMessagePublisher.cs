using EasyNetQ;
using EasyNetQ.NonGeneric;
using System;

namespace TauCode.Mq.EasyNetQ
{
    public class EasyNetQMessagePublisher : MessagePublisherBase
    {
        #region Fields

        private readonly string _connectionString;
        private IBus _bus;

        #endregion

        #region Constructor

        public EasyNetQMessagePublisher(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        #endregion

        #region Overridden

        protected override void StartImpl()
        {
            _bus = RabbitHutch.CreateBus(_connectionString);
        }

        protected override void PublishImpl(object message)
        {
            _bus.Publish(message.GetType(), message);
        }

        protected override void DisposeImpl()
        {
            _bus.Dispose();
        }

        #endregion
    }
}
