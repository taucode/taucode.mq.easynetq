﻿using Serilog;
using TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

namespace TauCode.Mq.EasyNetQ.LocalOldTests.Handlers.Hello.Async;

public class HelloAsyncHandler : MessageHandlerBase<HelloMessage>
{
    protected override async Task HandleAsyncImpl(HelloMessage message, CancellationToken cancellationToken = default)
    {
        var topicString = " (no topic)";
        if (message.Topic != null)
        {
            topicString = $" (topic: '{message.Topic}')";
        }

        await Task.Delay(message.MillisecondsTimeout, cancellationToken);

        Log.Information($"Hello async{topicString}, {message.Name}!");
        MessageRepository.Instance.Add(message);
    }
}