using NUnit.Framework;
using TauCode.Mq.EasyNetQ.Tests.ContextFactories;

namespace TauCode.Mq.EasyNetQ.Tests.Subscriber;

[TestFixture]
public partial class SubscriberTests
{
    #region ctor(IMessageHandlerContextFactory)

    [Test]
    public void Ctor_Arg1ValidFactory_CreatesSubscriber()
    {
        // Arrange
        var contextFactory = new GoodContextFactory();

        // Act
        using var subscriber = new EasyNetQMessageSubscriber(contextFactory);

        // Assert
        Assert.That(subscriber.ContextFactory, Is.SameAs(contextFactory));
        Assert.That(subscriber.ConnectionString, Is.Null);
    }

    [Test]
    public void Ctor_Arg1FactoryIsNull_ThrowsException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => new EasyNetQMessageSubscriber(null!))!;

        // Assert
        Assert.That(ex.ParamName, Is.EqualTo("contextFactory"));
    }

    #endregion

    #region ctor(IMessageHandlerContextFactory, ILogger)

    [Test]
    public void Ctor_Arg2ValidFactory_CreatesSubscriber()
    {
        // Arrange
        var contextFactory = new GoodContextFactory();

        // Act
        using var subscriber = new EasyNetQMessageSubscriber(contextFactory, _logger);

        // Assert
        Assert.That(subscriber.ContextFactory, Is.SameAs(contextFactory));
        Assert.That(subscriber.ConnectionString, Is.Null);
    }

    [Test]
    public void Ctor_Arg2FactoryIsNull_ThrowsException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => new EasyNetQMessageSubscriber(null!, _logger))!;

        // Assert
        Assert.That(ex.ParamName, Is.EqualTo("contextFactory"));
    }


    #endregion

    #region ctor(IMessageHandlerContextFactory, ILogger, string)

    [Test]
    public void Ctor_Arg3ValidFactory_CreatesSubscriber()
    {
        // Arrange
        var contextFactory = new GoodContextFactory();

        // Act
        using var subscriber1 = new EasyNetQMessageSubscriber(contextFactory, "host=localhost", _logger);
        using var subscriber2 = new EasyNetQMessageSubscriber(contextFactory, null, _logger);

        // Assert
        Assert.That(subscriber1.ContextFactory, Is.SameAs(contextFactory));
        Assert.That(subscriber1.ConnectionString, Is.EqualTo("host=localhost"));

        Assert.That(subscriber2.ContextFactory, Is.SameAs(contextFactory));
        Assert.That(subscriber2.ConnectionString, Is.Null);
    }

    [Test]
    public void Ctor_Arg3FactoryIsNull_ThrowsException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => new EasyNetQMessageSubscriber(null!, "host=localhost", _logger))!;

        // Assert
        Assert.That(ex.ParamName, Is.EqualTo("contextFactory"));
    }


    #endregion
}