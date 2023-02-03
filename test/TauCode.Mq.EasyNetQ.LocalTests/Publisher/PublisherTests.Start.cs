using NUnit.Framework;
using TauCode.Working;

namespace TauCode.Mq.EasyNetQ.LocalTests.Publisher;

[TestFixture]
public partial class PublisherTests
{
    [Test]
    public void Start_StateIsValid_StartsPublisher()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-pub",
        };

        // Act
        publisher.Start();

        // Assert
        var log = this.CurrentLog;

        Assert.That(log, Does.Contain("(EasyNetQMessagePublisher 'my-pub') Inside method 'Start'. About to call 'OnAfterStarted'."));
        Assert.That(publisher.State, Is.EqualTo(WorkerState.Running));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("bad_connection_string")]
    public void Start_ConnectionStringIsInvalid_StartsPublisher(string badConnectionString)
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher(_logger)
        {
            Name = "my-pub",
        };

        publisher.ConnectionString = badConnectionString;

        // Act
        Exception exception = null!;

        try
        {
            publisher.Start();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        // Assert
        var log = this.CurrentLog;
        Assert.That(exception, Is.Not.Null);
        Assert.That(log, Does.Contain("'OnBeforeStarting' has thrown an exception"));
        Assert.That(publisher, Has.Property(nameof(IWorker.State)).EqualTo(WorkerState.Stopped));
    }

    [Test]
    public void Start_AlreadyStarted_ThrowsException()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-pub",
        };
        publisher.Start();

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => publisher.Start());

        // Assert
        Assert.That(ex, Has.Message.StartWith("Cannot perform operation 'Start'. Worker state is 'Running'."));
    }

    [Test]
    public void Start_Disposed_ThrowsException()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-pub",
        };
        publisher.Dispose();

        // Act
        var ex = Assert.Throws<ObjectDisposedException>(() => publisher.Start())!;

        // Assert
        Assert.That(ex.ObjectName, Is.EqualTo("my-pub"));
    }
}