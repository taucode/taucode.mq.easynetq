using TauCode.Mq.EasyNetQ.OldTests.Messages;

namespace TauCode.Mq.EasyNetQ.OldTests.BadHandlers;

public class HelloAndByeAsyncHandler : IMessageHandler<HelloMessage>, IMessageHandler<ByeMessage>
{
    public void Handle(HelloMessage message)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(HelloMessage message, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public void Handle(ByeMessage message)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(ByeMessage message, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public void Handle(IMessage message)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(IMessage message, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }
}