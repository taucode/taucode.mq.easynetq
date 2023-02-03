using TauCode.Mq.EasyNetQ.LocalTests.Messages.Good;

namespace TauCode.Mq.EasyNetQ.LocalTests.Handlers.Bad;

public interface IInterfaceHandler : IMessageHandler<HelloMessage>
{
}