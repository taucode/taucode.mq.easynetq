using EasyNetQ;
using EasyNetQ.NonGeneric;
using System;
using System.Collections.Generic;
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

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                this.CheckStateForOperation(WorkerState.Stopped);
                _connectionString = value;
            }
        }

        protected override void SubscribeImpl(IEnumerable<ISubscriptionRequest> requests)
        {
            _bus = RabbitHutch.CreateBus(this.ConnectionString);

            foreach (var request in requests)
            {
                var subId = Guid.NewGuid().ToString(); // todo

                if (request.Topic == null)
                {
                    _bus.Subscribe(request.MessageType, subId, request.Handler);
                }
                else
                {
                    _bus.Subscribe(request.MessageType, subId, request.Handler, configuration => configuration.WithTopic(request.Topic));
                }
            }
        }

        protected override void UnsubscribeImpl()
        {
            _bus.Dispose();
            _bus = null;
        }
    }
}
