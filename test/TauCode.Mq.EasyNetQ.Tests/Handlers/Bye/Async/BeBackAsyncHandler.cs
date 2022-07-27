﻿using System.Threading;
using System.Threading.Tasks;
using Serilog;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.Handlers.Bye.Async
{
    public class BeBackAsyncHandler : AsyncMessageHandlerBase<ByeMessage>
    {
        public override async Task HandleAsync(ByeMessage message, CancellationToken cancellationToken)
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
}
