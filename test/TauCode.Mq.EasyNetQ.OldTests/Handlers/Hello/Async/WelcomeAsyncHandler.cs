using Serilog;
using TauCode.Mq.EasyNetQ.OldTests.Messages;

namespace TauCode.Mq.EasyNetQ.OldTests.Handlers.Hello.Async;

public class WelcomeAsyncHandler : MessageHandlerBase<HelloMessage>
{
    protected override async Task HandleAsyncImpl(HelloMessage message, CancellationToken cancellationToken = default)
    {
        var topicString = " (no topic)";
        if (message.Topic != null)
        {
            topicString = $" (topic: '{message.Topic}')";
        }

        await Task.Delay(message.MillisecondsTimeout, cancellationToken);

        Log.Information($"Welcome async{topicString}, {message.Name}!");
        MessageRepository.Instance.Add(message);
    }
}