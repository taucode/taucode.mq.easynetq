﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.Handlers.Hello.Async
{
    public class FishHaterAsyncHandler : AsyncMessageHandlerBase<HelloMessage>
    {
        public override async Task HandleAsync(HelloMessage message, CancellationToken cancellationToken)
        {
            var topicString = " (no topic)";
            if (message.Topic != null)
            {
                topicString = $" (topic: '{message.Topic}')";
            }

            if (message.Name.Contains("fish", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"I hate you async{topicString}, '{message.Name}'! Exception thrown!");
            }

            await Task.Delay(message.MillisecondsTimeout, cancellationToken);

            Log.Information($"Not fish - then hi async{topicString}, {message.Name}!");
            MessageRepository.Instance.Add(message);
        }
    }
}