using Serilog;
using TauCode.Mq.EasyNetQ.OldTests.Messages;

namespace TauCode.Mq.EasyNetQ.OldTests.BadHandlers;

public class DecayingMessageHandler : MessageHandlerBase<DecayingMessage>
{
    protected override Task HandleAsyncImpl(DecayingMessage message, CancellationToken cancellationToken = default)
    {
        Log.Information($"Decayed sync, {message.DecayedProperty}!");
        MessageRepository.Instance.Add(message);
        return Task.CompletedTask;
    }
}