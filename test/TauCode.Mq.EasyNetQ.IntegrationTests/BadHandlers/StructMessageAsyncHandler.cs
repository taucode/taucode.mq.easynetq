using System;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ.IntegrationTests.Messages;

namespace TauCode.Mq.EasyNetQ.IntegrationTests.BadHandlers
{
    public class StructMessageAsyncHandler : AsyncMessageHandlerBase<StructMessage>
    {
        public override Task HandleAsync(StructMessage message, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}
