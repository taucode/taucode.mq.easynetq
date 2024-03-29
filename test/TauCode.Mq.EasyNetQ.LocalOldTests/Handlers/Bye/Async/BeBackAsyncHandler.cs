﻿using Serilog;
using TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

namespace TauCode.Mq.EasyNetQ.LocalOldTests.Handlers.Bye.Async;

public class BeBackAsyncHandler : MessageHandlerBase<ByeMessage>
{
    protected override async Task HandleAsyncImpl(ByeMessage message, CancellationToken cancellationToken = default)
    {
        var topicString = " (no topic)";
        if (message.Topic != null)
        {
            topicString = $" (topic: '{message.Topic}')";
        }

        await Task.Delay(message.MillisecondsTimeout, cancellationToken);

        Log.Information($"Be back async{topicString}, {message.Nickname}!");
        MessageRepository.Instance.Add(message);
    }
}