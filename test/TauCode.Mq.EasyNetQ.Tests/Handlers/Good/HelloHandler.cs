using Serilog;
using TauCode.Mq.EasyNetQ.Tests.Messages.Good;

namespace TauCode.Mq.EasyNetQ.Tests.Handlers.Good;

public class HelloHandler : MessageHandlerBase<HelloMessage>
{
    protected override async Task HandleAsyncImpl(HelloMessage message, CancellationToken cancellationToken = default)
    {
        Log.Information($"Hello, {message.Name} (topic is {message.Topic.ToCaption()})!");
        await Task.CompletedTask;
    }
}