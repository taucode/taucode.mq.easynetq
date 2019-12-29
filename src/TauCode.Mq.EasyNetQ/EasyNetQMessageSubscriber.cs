using EasyNetQ;
using EasyNetQ.NonGeneric;
using System;
using TauCode.Working;

namespace TauCode.Mq.EasyNetQ
{
    public class EasyNetQMessageSubscriber : MessageSubscriberBase, IEasyNetQMessageSubscriber
    {
        private string _connectionString;
        private IBus _bus;

        public EasyNetQMessageSubscriber(IMessageHandlerContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        protected override void StartImpl()
        {
            base.StartImpl();
            _bus = RabbitHutch.CreateBus(this.ConnectionString);
            this.SubscribeBus();
        }

        private void SubscribeBus()
        {
            foreach (var pair in this.Bundles)
            {
                var subId = Guid.NewGuid().ToString(); // todo
                var bundle = pair.Value;
                var topic = bundle.Topic;

                if (topic == null)
                {
                    _bus.Subscribe(bundle.MessageType, subId, bundle.Handle);
                }
                else
                {
                    _bus.Subscribe(bundle.MessageType, subId, bundle.Handle, configuration => configuration.WithTopic(topic));
                }
            }
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
