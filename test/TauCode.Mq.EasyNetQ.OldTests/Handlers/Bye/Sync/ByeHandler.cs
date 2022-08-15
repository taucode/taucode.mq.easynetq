using Serilog;
using TauCode.Mq.EasyNetQ.OldTests.Messages;

namespace TauCode.Mq.EasyNetQ.OldTests.Handlers.Bye.Sync;

public class ByeHandler : MessageHandlerBase<ByeMessage>
{
    protected override Task HandleAsyncImpl(ByeMessage message, CancellationToken cancellationToken = default)
    {
        var topicString = " (no topic)";
        if (message.Topic != null)
        {
            topicString = $" (topic: '{message.Topic}')";
        }

        Log.Information($"Bye sync{topicString}, {message.Nickname}!");
        MessageRepository.Instance.Add(message);

        return Task.CompletedTask;
    }
}