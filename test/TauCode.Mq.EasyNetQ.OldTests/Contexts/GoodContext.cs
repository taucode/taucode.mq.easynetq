using Serilog;
using TauCode.Mq.EasyNetQ.OldTests.Handlers.Bye.Async;
using TauCode.Mq.EasyNetQ.OldTests.Handlers.Bye.Sync;
using TauCode.Mq.EasyNetQ.OldTests.Handlers.Hello.Async;
using TauCode.Mq.EasyNetQ.OldTests.Handlers.Hello.Sync;

namespace TauCode.Mq.EasyNetQ.OldTests.Contexts;

public class GoodContext : IMessageHandlerContext
{
    private static readonly HashSet<Type> SupportedHandlerTypes = new[]
        {
            typeof(BeBackAsyncHandler),
            typeof(ByeAsyncHandler),

            typeof(ByeHandler),

            typeof(CancelingHelloAsyncHandler),
            typeof(FaultingHelloAsyncHandler),
            typeof(FishHaterAsyncHandler),
            typeof(HelloAsyncHandler),
            typeof(WelcomeAsyncHandler),

            typeof(FishHaterHandler),
            typeof(HelloHandler),
            typeof(WelcomeHandler),
        }
        .ToHashSet();

    public Task BeginAsync(CancellationToken cancellationToken)
    {
        Log.Debug("Context began.");

        return Task.CompletedTask;
    }

    public object GetService(Type serviceType)
    {
        if (SupportedHandlerTypes.Contains(serviceType))
        {
            var ctor = serviceType.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new NotSupportedException($"Type '{serviceType.FullName}' has no parameterless constructor.");
            }

            var service = ctor.Invoke(new object[] { });
            return service;
        }

        throw new NotSupportedException($"Service of type '{serviceType.FullName}' is not supported.");
    }

    public Task EndAsync(CancellationToken cancellationToken)
    {
        Log.Debug("Context ended.");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        // idle
    }
}