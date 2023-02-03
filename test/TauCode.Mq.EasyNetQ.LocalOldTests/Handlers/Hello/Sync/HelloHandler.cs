using Serilog;
using TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

namespace TauCode.Mq.EasyNetQ.LocalOldTests.Handlers.Hello.Sync;

public class HelloHandler : MessageHandlerBase<HelloMessage>
{
    protected override Task HandleAsyncImpl(HelloMessage message, CancellationToken cancellationToken = default)
    {
        var topicString = " (no topic)";
        if (message.Topic != null)
        {
            topicString = $" (topic: '{message.Topic}')";
        }

        Log.Information($"Hello sync{topicString}, {message.Name}!");
        MessageRepository.Instance.Add(message);

        return Task.CompletedTask;
    }
}