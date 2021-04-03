using Serilog;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.BadHandlers
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
