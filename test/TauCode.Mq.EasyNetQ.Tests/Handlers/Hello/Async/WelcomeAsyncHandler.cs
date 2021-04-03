using System.Threading;
using System.Threading.Tasks;
using Serilog;
using TauCode.Mq.EasyNetQ.Tests.Messages;

namespace TauCode.Mq.EasyNetQ.Tests.Handlers.Hello.Async
{
    public class WelcomeAsyncHandler : AsyncMessageHandlerBase<HelloMessage>
    {
        public override async Task HandleAsync(HelloMessage message, CancellationToken cancellationToken)
        {
            var topicString = " (no topic)";
            if (message.Topic != null)
            {
                topicString = $" (topic: '{message.Topic}')";
            }

            await Task.Delay(message.MillisecondsTimeout, cancellationToken);

            Log.Information($"Welcome async{topicString}, {message.Name}!");
            MessageRepository.Instance.Add(message);
        }
    }
}
