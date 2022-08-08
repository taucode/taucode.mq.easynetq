using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.BadHandlers;

public abstract class AbstractHandler : MessageHandlerBase<HelloMessage>
{
}