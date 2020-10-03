using System;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ.IntegrationTests.Messages;

namespace TauCode.Mq.EasyNetQ.IntegrationTests.BadHandlers
{
    public class AbstractMessageAsyncHandler : AsyncMessageHandlerBase<AbstractMessage>
    {
        public override Task HandleAsync(AbstractMessage message, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}
