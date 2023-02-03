using TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

namespace TauCode.Mq.EasyNetQ.LocalOldTests.BadHandlers;

public class NotImplementingHandlerInterface
{
    public void Handle(HelloMessage message)
    {
        throw new NotSupportedException();
    }

    public void Handle(IMessage message)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(HelloMessage message, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(IMessage message, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }
}