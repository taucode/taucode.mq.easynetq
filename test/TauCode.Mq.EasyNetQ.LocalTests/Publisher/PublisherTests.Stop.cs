using NUnit.Framework;
using TauCode.Working;

namespace TauCode.Mq.EasyNetQ.LocalTests.Publisher;

[TestFixture]
public partial class PublisherTests
{
    [Test]
    public void Stop_Running_StopsPublisher()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-pub",
        };
        publisher.Start();

        // Act
        publisher.Stop();

        // Assert
        var log = this.CurrentLog;

        Assert.That(log, Does.Contain("(EasyNetQMessagePublisher 'my-pub') Inside method 'ShutdownImpl'. Bus is disposed."));
        Assert.That(log, Does.Contain("(EasyNetQMessagePublisher 'my-pub') Inside method 'Stop'. About to call 'OnAfterStopped'."));

        Assert.That(publisher.State, Is.EqualTo(WorkerState.Stopped));
    }

    [Test]
    public void Stop_NotStarted_ThrowsException()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-pub",
        };

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => publisher.Stop());

        // Assert
        Assert.That(publisher, Has.Property(nameof(IWorker.State)).EqualTo(WorkerState.Stopped));
        Assert.That(ex, Has.Message.EqualTo("Cannot perform operation 'Stop'. Worker state is 'Stopped'. Worker name is 'my-pub'."));
    }

    [Test]
    public void Stop_Stopped_ThrowsException()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-pub",
        };

        publisher.Start();
        publisher.Stop();

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => publisher.Stop());

        // Assert
        Assert.That(publisher, Has.Property(nameof(IWorker.State)).EqualTo(WorkerState.Stopped));
        Assert.That(ex, Has.Message.EqualTo("Cannot perform operation 'Stop'. Worker state is 'Stopped'. Worker name is 'my-pub'."));
    }

    [Test]
    public void Stop_Disposed_ThrowsException()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-pub",
        };

        publisher.Dispose();

        // Act
        var ex = Assert.Throws<ObjectDisposedException>(() => publisher.Stop());

        // Assert
        Assert.That(publisher, Has.Property(nameof(IWorker.State)).EqualTo(WorkerState.Stopped));
        Assert.That(ex.ObjectName, Is.EqualTo("my-pub"));
    }
}