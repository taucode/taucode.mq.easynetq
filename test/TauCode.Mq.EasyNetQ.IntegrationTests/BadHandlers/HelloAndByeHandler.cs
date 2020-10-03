using System;
using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ.IntegrationTests.Messages;

namespace TauCode.Mq.EasyNetQ.IntegrationTests.BadHandlers
{
    public class HelloAndByeHandler : IMessageHandler<HelloMessage>, IMessageHandler<ByeMessage>
    {
        public void Handle(HelloMessage message)
        {
            throw new NotSupportedException();
        }

        public void Handle(ByeMessage message)
        {
            throw new NotSupportedException();
        }

        public void Handle(object message)
        {
            throw new NotSupportedException();
        }
    }
}
