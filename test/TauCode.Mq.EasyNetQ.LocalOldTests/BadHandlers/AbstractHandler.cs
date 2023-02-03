using TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

namespace TauCode.Mq.EasyNetQ.LocalOldTests.BadHandlers;

public abstract class AbstractHandler : MessageHandlerBase<HelloMessage>
{
}