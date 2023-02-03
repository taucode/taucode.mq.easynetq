using NUnit.Framework;
using TauCode.Mq.EasyNetQ.LocalTests.ContextFactories;
using TauCode.Working;

namespace TauCode.Mq.EasyNetQ.LocalTests.Subscriber;

[TestFixture]
public partial class SubscriberTests
{
    [Test]
    public void Start_StateIsValid_StartsSubscriber()
    {
        // Arrange
        IMessageHandlerContextFactory contextFactory = new GoodContextFactory();

        using var subscriber = new EasyNetQMessageSubscriber(contextFactory, "host=localhost", _logger)
        {
            Name = "my-sub",
        };

        // Act
        subscriber.Start();

        // Assert
        var log = this.CurrentLog;

        Assert.That(log, Does.Contain("(EasyNetQMessageSubscriber 'my-sub') Inside method 'Start'. About to call 'OnAfterStarted'."));
        Assert.That(subscriber.State, Is.EqualTo(WorkerState.Running));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("bad_connection_string")]
    public void Start_ConnectionStringIsInvalid_StartsSubscriber(string badConnectionString)
    {
        // Arrange
        IMessageHandlerContextFactory contextFactory = new GoodContextFactory();

        using var subscriber = new EasyNetQMessageSubscriber(contextFactory, _logger)
        {
            Name = "my-sub",
        };

        subscriber.ConnectionString = badConnectionString;

        // Act
        Exception exception = null!;

        try
        {
            subscriber.Start();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        // Assert
        var log = this.CurrentLog;
        Assert.That(exception, Is.Not.Null);
        Assert.That(log, Does.Contain("'OnBeforeStarting' has thrown an exception"));
        Assert.That(subscriber.State, Is.EqualTo(WorkerState.Stopped));
    }

    [Test]
    public void Start_AlreadyStarted_ThrowsException()
    {
        // Arrange
        IMessageHandlerContextFactory contextFactory = new GoodContextFactory();

        using var subscriber = new EasyNetQMessageSubscriber(contextFactory, "host=localhost", _logger)
        {
            Name = "my-sub",
        };
        subscriber.Start();

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => subscriber.Start());

        // Assert
        Assert.That(ex, Has.Message.StartWith("Cannot perform operation 'Start'. Worker state is 'Running'."));
    }

    [Test]
    public void Start_Disposed_ThrowsException()
    {
        // Arrange
        IMessageHandlerContextFactory contextFactory = new GoodContextFactory();

        using var subscriber = new EasyNetQMessageSubscriber(contextFactory, "host=localhost", _logger)
        {
            Name = "my-sub",
        };
        subscriber.Dispose();

        // Act
        var ex = Assert.Throws<ObjectDisposedException>(() => subscriber.Start())!;

        // Assert
        Assert.That(ex.ObjectName, Is.EqualTo("my-sub"));
    }
}