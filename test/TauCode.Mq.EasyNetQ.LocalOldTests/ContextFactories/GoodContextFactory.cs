using TauCode.Mq.EasyNetQ.LocalOldTests.Contexts;

namespace TauCode.Mq.EasyNetQ.LocalOldTests.ContextFactories;

public class GoodContextFactory : IMessageHandlerContextFactory
{
    public IMessageHandlerContext CreateContext()
    {
        return new GoodContext();
    }
}