using TauCode.Mq.EasyNetQ.LocalTests.Messages.Bad;

namespace TauCode.Mq.EasyNetQ.LocalTests.Handlers.Bad;

public class HandlerWhichImplementsInterfaceMessage : MessageHandlerBase<IInterfaceMessage>
{
    protected override Task HandleAsyncImpl(IInterfaceMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}