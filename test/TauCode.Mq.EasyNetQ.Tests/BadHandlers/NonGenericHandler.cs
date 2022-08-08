using TauCode.Mq.Abstractions;

namespace TauCode.Mq.EasyNetQ.Tests.BadHandlers;

public class NonGenericHandler : IMessageHandler
{
    public void Handle(IMessage message)
    {
        throw new NotSupportedException();
    }
}