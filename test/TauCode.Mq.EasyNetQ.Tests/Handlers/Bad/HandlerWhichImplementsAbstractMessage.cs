using TauCode.Mq.EasyNetQ.Tests.Messages.Bad;

namespace TauCode.Mq.EasyNetQ.Tests.Handlers.Bad;

public class HandlerWhichImplementsAbstractMessage : MessageHandlerBase<AbstractMessage>
{
    protected override Task HandleAsyncImpl(AbstractMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}