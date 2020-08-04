using EasyNetQ.NonGeneric;
using TauCode.Mq.Abstractions;
using TauCode.Working;
using IBus = EasyNetQ.IBus;
using RabbitHutch = EasyNetQ.RabbitHutch;

namespace TauCode.Mq.EasyNetQ
{
    public class EasyNetQMessagePublisher : MessagePublisherBase, IEasyNetQMessagePublisher
    {
        private string _connectionString;
        private IBus _bus;

        protected override void StartImpl()
        {
            base.StartImpl();
            _bus = RabbitHutch.CreateBus(this.ConnectionString);
        }

        protected override void StopImpl()
        {
            base.StopImpl();
            _bus.Dispose();
            _bus = null;
        }

        protected override void DisposeImpl()
        {
            base.DisposeImpl();
            _bus.Dispose();
            _bus = null;
        }

        protected override void PublishImpl(IMessage message)
        {
            _bus.Publish(message.GetType(), message);
        }

        protected override void PublishImpl(IMessage message, string topic)
        {
            _bus.Publish(message.GetType(), message, topic);
        }

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                this.CheckStateForOperation(WorkerState.Stopped);
                _connectionString = value;
            }
        }
    }
}
