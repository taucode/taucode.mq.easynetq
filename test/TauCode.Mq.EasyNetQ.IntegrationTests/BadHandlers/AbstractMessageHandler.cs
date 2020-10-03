using System;
using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ.IntegrationTests.Messages;

namespace TauCode.Mq.EasyNetQ.IntegrationTests.BadHandlers
{
    public class AbstractMessageHandler : MessageHandlerBase<AbstractMessage>
    {
        public override void Handle(AbstractMessage message)
        {
            throw new NotSupportedException();
        }
    }
}
