using TauCode.Mq.EasyNetQ.Tests.Messages.Bad;

namespace TauCode.Mq.EasyNetQ.Tests.Handlers.Bad;

public class HandlerWhichImplementsInterfaceMessage : MessageHandlerBase<IInterfaceMessage>
{
    protected override Task HandleAsyncImpl(IInterfaceMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}