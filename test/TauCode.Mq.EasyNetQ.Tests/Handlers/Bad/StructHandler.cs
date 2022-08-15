using TauCode.Mq.EasyNetQ.Tests.Messages.Good;

namespace TauCode.Mq.EasyNetQ.Tests.Handlers.Bad;

public struct StructHandler : IMessageHandler<HelloMessage>
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