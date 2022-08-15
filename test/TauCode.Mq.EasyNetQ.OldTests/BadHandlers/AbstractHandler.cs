using TauCode.Mq.EasyNetQ.OldTests.Messages;

namespace TauCode.Mq.EasyNetQ.OldTests.BadHandlers;

public abstract class AbstractHandler : MessageHandlerBase<HelloMessage>
{
}