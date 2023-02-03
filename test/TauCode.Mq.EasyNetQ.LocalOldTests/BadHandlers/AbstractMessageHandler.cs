using TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

namespace TauCode.Mq.EasyNetQ.LocalOldTests.BadHandlers;

public class AbstractMessageHandler : MessageHandlerBase<AbstractMessage>
{
    protected override Task HandleAsyncImpl(AbstractMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}