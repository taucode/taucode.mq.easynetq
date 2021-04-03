using System;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.BadHandlers
{
    public class StructMessageHandler : MessageHandlerBase<StructMessage>
    {
        public override void Handle(StructMessage message)
        {
            throw new NotSupportedException();
        }
    }
}
