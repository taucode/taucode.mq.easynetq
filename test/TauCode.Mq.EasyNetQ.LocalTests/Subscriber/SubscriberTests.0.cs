using System.Text;
using EasyNetQ;
using NUnit.Framework;
using Serilog;
using TauCode.IO;

namespace TauCode.Mq.EasyNetQ.LocalTests.Subscriber;

[TestFixture]
public partial class SubscriberTests
{
    private const string DefaultConnectionString = "host=localhost"; // todo: use this constant instead of hardcode.

    private ILogger _logger = null!;
    private StringWriterWithEncoding _writer = null!;

    private IBus? _publisherBus;

    [SetUp]
    public void SetUp()
    {
        _writer = new StringWriterWithEncoding(Encoding.UTF8);
        _logger = new LoggerConfiguration()
            .WriteTo.TextWriter(
                _writer,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}]{ObjectTag} {Message}{NewLine}{Exception}"
            )
            .MinimumLevel.Verbose()
            .CreateLogger();
        Log.Logger = _logger;
    }

    [TearDown]
    public void TearDown()
    {
        _publisherBus?.Dispose();
        _publisherBus = null;
    }

    protected string CurrentLog => _writer.ToString();

    protected void PublishTestMessage(IMessage message)
    {
        if (_publisherBus == null)
        {
            _publisherBus = RabbitHutch.CreateBus(
                DefaultConnectionString,
                x => x.EnableSerilogLogging(_logger));
        }

        if (message.Topic == null)
        {
            _publisherBus.PubSub.Publish(message, message.GetType());
        }
        else
        {
            _publisherBus.PubSub.Publish(message, message.GetType(), message.Topic);
        }
    }
}