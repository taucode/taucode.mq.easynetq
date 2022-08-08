using Serilog;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.Handlers.Hello.Sync;

public class HelloHandler : MessageHandlerBase<HelloMessage>
{
    public override void Handle(HelloMessage message)
    {
        var topicString = " (no topic)";
        if (message.Topic != null)
        {
            topicString = $" (topic: '{message.Topic}')";
        }

        Log.Information($"Hello sync{topicString}, {message.Name}!");
        MessageRepository.Instance.Add(message);
    }
}