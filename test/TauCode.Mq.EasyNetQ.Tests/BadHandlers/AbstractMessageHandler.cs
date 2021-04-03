using System;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.BadHandlers
{
    public class AbstractMessageHandler : MessageHandlerBase<AbstractMessage>
    {
        public override void Handle(AbstractMessage message)
        {
            throw new NotSupportedException();
        }
    }
}
