namespace TauCode.Mq.EasyNetQ.LocalTests.Handlers.Bad;

public class NotGenericInterfaceHandler : IMessageHandler
{
    public Task HandleAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}