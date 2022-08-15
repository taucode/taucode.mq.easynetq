using NUnit.Framework;

namespace TauCode.Mq.EasyNetQ.Tests.Publisher;

[TestFixture]
public partial class PublisherTests
{
    #region ctor()

    [Test]
    public void Ctor_NoArguments_CreatesPublisher()
    {
        // Arrange

        // Act
        using var publisher = new EasyNetQMessagePublisher();
        var connectionStringAfterCreation = publisher.ConnectionString;

        publisher.ConnectionString = "host=localhost";
        publisher.Start(); // to invoke logging; will do nothing since publisher's logger is null

        // Assert
        var log = this.CurrentLog;
        Assert.That(log, Is.Empty);
        Assert.That(connectionStringAfterCreation, Is.Null);
    }

    #endregion

    #region ctor(Ilogger)

    [Test]
    public void Ctor_LoggerIsNull_CreatesPublisher()
    {
        // Arrange

        // Act
        using var publisher = new EasyNetQMessagePublisher(null);
        var connectionStringAfterCreation = publisher.ConnectionString;

        publisher.ConnectionString = "host=localhost";
        publisher.Start(); // to invoke logging; will do nothing since publisher's logger is null

        // Assert
        var log = this.CurrentLog;
        Assert.That(log, Is.Empty);
        Assert.That(connectionStringAfterCreation, Is.Null);
    }

    [Test]
    public void Ctor_ValidLogger_CreatesPublisher()
    {
        // Arrange

        // Act
        using var publisher = new EasyNetQMessagePublisher(_logger)
        {
            Name = "my-pub",
        };

        var connectionStringAfterCreation = publisher.ConnectionString;

        publisher.ConnectionString = "host=localhost";
        publisher.Start();

        // Assert
        var log = this.CurrentLog;
        Assert.That(log, Is.Not.Empty);
        Assert.That(connectionStringAfterCreation, Is.Null);
    }

    #endregion

    #region ctor(string, ILogger)

    [Test]
    [TestCase(null)]
    [TestCase("host=localhost")]
    [TestCase("some_bad_connection_string")]
    public void Ctor_LoggerAndConnectionString_CreatesPublisher(string connectionString)
    {
        // Arrange

        // Act
        using var publisher = new EasyNetQMessagePublisher(connectionString, _logger)
        {
            Name = "my-pub",
        };

        // Assert
        var log = this.CurrentLog;
        Assert.That(publisher.ConnectionString, Is.EqualTo(connectionString));
    }

    #endregion
}