using TauCode.Mq.EasyNetQ.IntegrationTests.Contexts;

namespace TauCode.Mq.EasyNetQ.IntegrationTests.ContextFactories
{
    public class GoodContextFactory : IMessageHandlerContextFactory
    {
        public IMessageHandlerContext CreateContext()
        {
            return new GoodContext();
        }
    }
}
