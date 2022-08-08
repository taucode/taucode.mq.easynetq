using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.BadHandlers;

public class HelloAndByeAsyncHandler : IAsyncMessageHandler<HelloMessage>, IAsyncMessageHandler<ByeMessage>
{
    public Task HandleAsync(HelloMessage message, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(ByeMessage message, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(IMessage message, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }
}