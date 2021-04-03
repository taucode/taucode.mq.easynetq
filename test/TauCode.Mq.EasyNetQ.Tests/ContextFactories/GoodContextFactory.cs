using TauCode.Mq.EasyNetQ.Tests.Contexts;

namespace TauCode.Mq.EasyNetQ.Tests.ContextFactories
{
    public class GoodContextFactory : IMessageHandlerContextFactory
    {
        public IMessageHandlerContext CreateContext()
        {
            return new GoodContext();
        }
    }
}
