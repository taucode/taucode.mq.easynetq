using TauCode.Mq.EasyNetQ.LocalTests.Messages.Good;

namespace TauCode.Mq.EasyNetQ.LocalTests.Handlers.Bad;

public class NoInterfaceHandler
{
    public Task HandleAsync(HelloMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}