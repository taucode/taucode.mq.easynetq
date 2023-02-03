using TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

namespace TauCode.Mq.EasyNetQ.LocalOldTests.Handlers.Hello.Async;

public class FaultingHelloAsyncHandler : MessageHandlerBase<HelloMessage>
{
    protected override async Task HandleAsyncImpl(HelloMessage message, CancellationToken cancellationToken = default)
    {
        var topicString = " (no topic)";
        if (message.Topic != null)
        {
            topicString = $" (topic: '{message.Topic}')";
        }

        await Task.Delay(20, cancellationToken);
        throw new NotSupportedException($"Sorry, I am faulting async{topicString}, {message.Name}...");
    }
}