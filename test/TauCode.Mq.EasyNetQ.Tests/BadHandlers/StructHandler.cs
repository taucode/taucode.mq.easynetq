using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.BadHandlers;

public struct StructHandler : IMessageHandler<HelloMessage>
{
    public void Handle(HelloMessage message)
    {
        throw new NotSupportedException();
    }

    public void Handle(IMessage message)
    {
        throw new NotSupportedException();
    }
}