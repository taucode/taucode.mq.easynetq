using System;
using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ.IntegrationTests.Messages;

namespace TauCode.Mq.EasyNetQ.IntegrationTests.BadHandlers
{
    public class StructMessageHandler : MessageHandlerBase<StructMessage>
    {
        public override void Handle(StructMessage message)
        {
            throw new NotSupportedException();
        }
    }
}
