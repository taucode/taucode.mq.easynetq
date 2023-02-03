using Serilog;
using TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

namespace TauCode.Mq.EasyNetQ.LocalOldTests.BadHandlers;

public class DecayingMessageHandler : MessageHandlerBase<DecayingMessage>
{
    protected override Task HandleAsyncImpl(DecayingMessage message, CancellationToken cancellationToken = default)
    {
        Log.Information($"Decayed sync, {message.DecayedProperty}!");
        MessageRepository.Instance.Add(message);
        return Task.CompletedTask;
    }
}