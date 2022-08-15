using NUnit.Framework;
using TauCode.Working;

namespace TauCode.Mq.EasyNetQ.Tests.Publisher;

[TestFixture]
public partial class PublisherTests
{
    [Test]
    public void Dispose_NotStarted_Disposes()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-pub",
        };

        // Act
        publisher.Dispose();

        // Assert
        var log = this.CurrentLog;

        Assert.That(publisher.State, Is.EqualTo(WorkerState.Stopped));
        Assert.That(publisher.IsDisposed, Is.True);

        Assert.That(log, Does.Contain("(EasyNetQMessagePublisher 'my-pub') Inside method 'Dispose'. About to call 'OnAfterDisposed'."));
    }

    [Test]
    public void Dispose_Stopped_Disposes()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-pub",
        };

        publisher.Start();
        publisher.Stop();

        // Act
        publisher.Dispose();

        // Assert
        var log = this.CurrentLog;

        Assert.That(publisher.State, Is.EqualTo(WorkerState.Stopped));
        Assert.That(publisher.IsDisposed, Is.True);

        Assert.That(log, Does.Contain("(EasyNetQMessagePublisher 'my-pub') Inside method 'Dispose'. About to call 'OnAfterDisposed'."));
    }

    [Test]
    public void Dispose_Started_Disposes()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-pub",
        };

        publisher.Start();

        // Act
        publisher.Dispose();

        // Assert
        var log = this.CurrentLog;

        Assert.That(publisher.State, Is.EqualTo(WorkerState.Stopped));
        Assert.That(publisher.IsDisposed, Is.True);

        Assert.That(log, Does.Contain("(EasyNetQMessagePublisher 'my-pub') Inside method 'Stop'. About to call 'OnBeforeStopping'."));
        Assert.That(log, Does.Contain("(EasyNetQMessagePublisher 'my-pub') Inside method 'Stop'. About to call 'OnAfterStopped'."));
        Assert.That(log, Does.Contain("(EasyNetQMessagePublisher 'my-pub') Inside method 'Dispose'. About to call 'OnAfterDisposed'."));
    }

    [Test]
    public void Dispose_Disposed_DoesNothing()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-pub",
        };

        publisher.Dispose();

        // Act
        publisher.Dispose();

        // Assert
        var log = this.CurrentLog;

        Assert.That(publisher.State, Is.EqualTo(WorkerState.Stopped));
        Assert.That(publisher.IsDisposed, Is.True);

        var lines = log.Split(Environment.NewLine);

        var disposedLines = lines.Where(x => x.Contains("OnAfterDisposed")).ToList();

        Assert.That(disposedLines, Has.Count.EqualTo(1)); // doesn't dispose twice.
    }
}