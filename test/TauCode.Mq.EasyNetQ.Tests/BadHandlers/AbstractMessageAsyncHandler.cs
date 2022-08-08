using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.BadHandlers;

public class AbstractMessageAsyncHandler : AsyncMessageHandlerBase<AbstractMessage>
{
    public override Task HandleAsync(AbstractMessage message, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }
}