using TauCode.Mq.EasyNetQ.LocalTests.Messages.Bad;

namespace TauCode.Mq.EasyNetQ.LocalTests.Handlers.Bad;

public class HandlerWhichImplementsAbstractMessage : MessageHandlerBase<AbstractMessage>
{
    protected override Task HandleAsyncImpl(AbstractMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}