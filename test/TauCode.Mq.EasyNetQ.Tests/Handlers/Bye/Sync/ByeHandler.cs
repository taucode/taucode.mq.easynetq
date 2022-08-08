using Serilog;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.Handlers.Bye.Sync;

public class ByeHandler : MessageHandlerBase<ByeMessage>
{
    public override void Handle(ByeMessage message)
    {
        var topicString = " (no topic)";
        if (message.Topic != null)
        {
            topicString = $" (topic: '{message.Topic}')";
        }

        Log.Information($"Bye sync{topicString}, {message.Nickname}!");
        MessageRepository.Instance.Add(message);
    }
}