using System;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.BadHandlers
{
    public class StructMessageAsyncHandler : AsyncMessageHandlerBase<StructMessage>
    {
        public override Task HandleAsync(StructMessage message, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}
