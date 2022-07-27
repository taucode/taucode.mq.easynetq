using System;
using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.BadHandlers
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

        public void Handle(IMessage message)
        {
            throw new NotSupportedException();
        }
    }
}
