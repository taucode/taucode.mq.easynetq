using TauCode.Mq.EasyNetQ.LocalTests.Contexts;

namespace TauCode.Mq.EasyNetQ.LocalTests.ContextFactories;

public class GoodContextFactory : IMessageHandlerContextFactory
{
    public IMessageHandlerContext CreateContext()
    {
        return new GoodContext();
    }
}