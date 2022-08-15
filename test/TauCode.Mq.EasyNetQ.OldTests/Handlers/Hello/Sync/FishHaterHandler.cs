using Serilog;
using TauCode.Mq.EasyNetQ.OldTests.Messages;

namespace TauCode.Mq.EasyNetQ.OldTests.Handlers.Hello.Sync;

public class FishHaterHandler : MessageHandlerBase<HelloMessage>
{
    protected override Task HandleAsyncImpl(HelloMessage message, CancellationToken cancellationToken = default)
    {
        var topicString = " (no topic)";
        if (message.Topic != null)
        {
            topicString = $" (topic: '{message.Topic}')";
        }

        if (message.Name.Contains("fish", StringComparison.InvariantCultureIgnoreCase))
        {
            throw new Exception($"I hate you sync{topicString}, '{message.Name}'! Exception thrown!");
        }

        Log.Information($"Not fish - then hi sync{topicString}, {message.Name}!");
        MessageRepository.Instance.Add(message);

        return Task.CompletedTask;
    }
}