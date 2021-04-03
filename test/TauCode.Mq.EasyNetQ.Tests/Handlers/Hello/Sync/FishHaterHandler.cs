using System;
using Serilog;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.Handlers.Hello.Sync
{
    public class FishHaterHandler : MessageHandlerBase<HelloMessage>
    {
        public override void Handle(HelloMessage message)
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
        }
    }
}
