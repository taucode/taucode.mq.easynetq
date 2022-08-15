using TauCode.Mq.EasyNetQ.OldTests.Messages;

namespace TauCode.Mq.EasyNetQ.OldTests.BadHandlers;

public class AbstractMessageAsyncHandler : MessageHandlerBase<AbstractMessage>
{
    protected override Task HandleAsyncImpl(AbstractMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}