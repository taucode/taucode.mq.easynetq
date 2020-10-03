using System.Threading;
using System.Threading.Tasks;
using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ.IntegrationTests.Messages;

namespace TauCode.Mq.EasyNetQ.IntegrationTests.Handlers.Hello.Async
{
    public class CancelingHelloAsyncHandler : AsyncMessageHandlerBase<HelloMessage>
    {
        public override async Task HandleAsync(HelloMessage message, CancellationToken cancellationToken)
        {
            var topicString = " (no topic)";
            if (message.Topic != null)
            {
                topicString = $" (topic: '{message.Topic}')";
            }

            await Task.Delay(20, cancellationToken);
            throw new TaskCanceledException($"Sorry, I am cancelling async{topicString}, {message.Name}...");
        }
    }
}
