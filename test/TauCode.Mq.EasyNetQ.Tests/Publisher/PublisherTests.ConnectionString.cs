using NUnit.Framework;

namespace TauCode.Mq.EasyNetQ.Tests.Publisher;

[TestFixture]
public partial class PublisherTests
{
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("host=localhost")]
    [TestCase("bad_connection_string")]
    public void ConnectionString_JustCreated_SetsConnectionString(string connectionString)
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher();

        // Act
        publisher.ConnectionString = connectionString;

        // Assert
        var log = this.CurrentLog;
        Assert.That(publisher.ConnectionString, Is.EqualTo(connectionString));
    }

    [Test]
    public void ConnectionString_Started_ThrowsException()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher()
        {
            ConnectionString = "host=localhost",
            Name = "my-pub",
        };

        publisher.Start();

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => publisher.ConnectionString = "host=127.0.0.1")!;
        var connectionString = publisher.ConnectionString;

        // Assert
        Assert.That(ex.Message, Is.EqualTo("Cannot perform operation 'set ConnectionString'. Worker state is 'Running'. Worker name is 'my-pub'."));
        Assert.That(connectionString, Is.EqualTo("host=localhost"));
    }

    [Test]
    public void ConnectionString_ReStarted_ThrowsException()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher
        {
            ConnectionString = "host=localhost",
            Name = "my-pub",
        };

        publisher.Start();
        publisher.Stop();
        publisher.Start();

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => publisher.ConnectionString = "host=127.0.0.1")!;
        var connectionString = publisher.ConnectionString;

        // Assert
        Assert.That(ex.Message, Is.EqualTo("Cannot perform operation 'set ConnectionString'. Worker state is 'Running'. Worker name is 'my-pub'."));
        Assert.That(connectionString, Is.EqualTo("host=localhost"));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("host=localhost")]
    [TestCase("bad_connection_string")]
    public void ConnectionString_Stopped_SetsConnectionString(string connectionString)
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher();
        publisher.ConnectionString = "host=localhost";

        publisher.Start();
        publisher.Stop();

        // Act
        publisher.ConnectionString = connectionString;

        // Assert
        Assert.That(publisher.ConnectionString, Is.EqualTo(connectionString));
    }

    [Test]
    public void ConnectionString_Disposed_ThrowsException()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher();
        publisher.ConnectionString = "host=localhost";
        publisher.Name = "my-pub";

        publisher.Dispose();

        // Act
        var ex = Assert.Throws<ObjectDisposedException>(() => publisher.ConnectionString = "host=127.0.0.1")!;
        var connectionString = publisher.ConnectionString;

        // Assert
        Assert.That(connectionString, Is.EqualTo("host=localhost"));
    }
}