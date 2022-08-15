using TauCode.Mq.EasyNetQ.OldTests.Contexts;

namespace TauCode.Mq.EasyNetQ.OldTests.ContextFactories;

public class GoodContextFactory : IMessageHandlerContextFactory
{
    public IMessageHandlerContext CreateContext()
    {
        return new GoodContext();
    }
}