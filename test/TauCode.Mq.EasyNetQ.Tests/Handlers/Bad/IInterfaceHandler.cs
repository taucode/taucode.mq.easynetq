using TauCode.Mq.EasyNetQ.Tests.Messages.Good;

namespace TauCode.Mq.EasyNetQ.Tests.Handlers.Bad;

public interface IInterfaceHandler : IMessageHandler<HelloMessage>
{
}