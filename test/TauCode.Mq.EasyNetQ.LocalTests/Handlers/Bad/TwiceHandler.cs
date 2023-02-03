using TauCode.Mq.EasyNetQ.LocalTests.Messages.Good;

namespace TauCode.Mq.EasyNetQ.LocalTests.Handlers.Bad;

public class TwiceHandler : MessageHandlerBase<HelloMessage>, IMessageHandler<ByeMessage>
{
    protected override Task HandleAsyncImpl(HelloMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(ByeMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}