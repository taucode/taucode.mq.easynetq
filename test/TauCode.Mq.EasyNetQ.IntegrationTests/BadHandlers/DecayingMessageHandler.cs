using Serilog;
using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ.IntegrationTests.Messages;

namespace TauCode.Mq.EasyNetQ.IntegrationTests.BadHandlers
{
    public class DecayingMessageHandler : MessageHandlerBase<DecayingMessage>
    {
        public override void Handle(DecayingMessage message)
        {
            Log.Information($"Decayed sync, {message.DecayedProperty}!");
            MessageRepository.Instance.Add(message);
        }
    }
}
