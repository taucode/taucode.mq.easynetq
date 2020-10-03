using System.Threading;
using System.Threading.Tasks;
using Serilog;
using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ.IntegrationTests.Messages;

namespace TauCode.Mq.EasyNetQ.IntegrationTests.Handlers.Bye.Async
{
    public class ByeAsyncHandler : AsyncMessageHandlerBase<ByeMessage>
    {
        public override async Task HandleAsync(ByeMessage message, CancellationToken cancellationToken)
        {
            var topicString = " (no topic)";
            if (message.Topic != null)
            {
                topicString = $" (topic: '{message.Topic}')";
            }

            await Task.Delay(message.MillisecondsTimeout, cancellationToken);

            Log.Information($"Bye async{topicString}, {message.Nickname}!");
            MessageRepository.Instance.Add(message);
        }
    }
}
