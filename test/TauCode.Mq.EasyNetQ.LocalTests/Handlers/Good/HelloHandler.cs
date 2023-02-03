using Serilog;
using TauCode.Mq.EasyNetQ.LocalTests.Messages.Good;

namespace TauCode.Mq.EasyNetQ.LocalTests.Handlers.Good;

public class HelloHandler : MessageHandlerBase<HelloMessage>
{
    protected override async Task HandleAsyncImpl(HelloMessage message, CancellationToken cancellationToken = default)
    {
        Log.Information($"Hello, {message.Name} (topic is {message.Topic.ToCaption()})!");
        await Task.CompletedTask;
    }
}