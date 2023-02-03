using System.Text;
using NUnit.Framework;
using Serilog;
using TauCode.IO;

namespace TauCode.Mq.EasyNetQ.LocalTests.Publisher;

[TestFixture]
public partial class PublisherTests
{
    private ILogger _logger = null!;
    private StringWriterWithEncoding _writer = null!;

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
    }

    protected string CurrentLog => _writer.ToString();

    // todo: not used
    private static IEasyNetQMessagePublisher CreatePublisher(ILogger logger, bool? goodConnectionString, bool setName)
    {
        string? connectionString = null;

        if (goodConnectionString.HasValue)
        {
            if (goodConnectionString.Value)
            {
                connectionString = "host=localhost";
            }
            else
            {
                connectionString = "some_bad_connection_string";
            }
        }

        IEasyNetQMessagePublisher publisher = new EasyNetQMessagePublisher(connectionString, logger);
        if (setName)
        {
            publisher.Name = "my-pub";
        }

        return publisher;
    }

    [Test]
    public void Pause_NoArguments_ThrowsException()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher();

        // Act
        var ex = Assert.Throws<NotSupportedException>(() => publisher.Pause());

        // Assert
        Assert.That(ex.Message, Is.EqualTo("Pausing/resuming is not supported."));
    }

    [Test]
    public void Resume_NoArguments_ThrowsException()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher();

        // Act
        var ex = Assert.Throws<NotSupportedException>(() => publisher.Resume())!;

        // Assert
        Assert.That(ex.Message, Is.EqualTo("Pausing/resuming is not supported."));
    }
}