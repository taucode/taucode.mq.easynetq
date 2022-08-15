using NUnit.Framework;
using TauCode.Mq.EasyNetQ.Tests.ContextFactories;
using TauCode.Mq.EasyNetQ.Tests.Handlers.Bad;
using TauCode.Mq.EasyNetQ.Tests.Handlers.Good;
using TauCode.Mq.EasyNetQ.Tests.Messages.Good;
using TauCode.Mq.Exceptions;

namespace TauCode.Mq.EasyNetQ.Tests.Subscriber;

// todo: stop => subscriptions are kept
// todo: stop => no message handling

[TestFixture]
public partial class SubscriberTests
{
    [Test]
    public async Task SubscribeType_ValidArg_Subscribes()
    {
        // Arrange
        using var subscriber = this.CreateStandardMessageSubscriber();

        subscriber.Subscribe(typeof(HelloHandler));
        subscriber.Start();

        this.PublishTestMessage(
            new HelloMessage
            {
                Name = "Lesia"
            });

        // message with topic will also be intercepted
        this.PublishTestMessage(
            new HelloMessage
            {
                Name = "Olia",
                Topic = "let`s go to picnic",
            });

        await Task.Delay(300); // allow messages to be dispatched

        // Assert
        var log = this.CurrentLog;
        Assert.That(log, Does.Contain("Hello, Lesia (topic is <null>)!"));
        Assert.That(log, Does.Contain("Hello, Olia (topic is 'let`s go to picnic')!"));

        var subscriptions = subscriber.GetSubscriptions();
        Assert.That(subscriptions, Has.Count.EqualTo(1));

        var subscription = subscriptions.Single();

        Assert.That(subscription.MessageType, Is.EqualTo(typeof(HelloMessage)));
        Assert.That(subscription.Topic, Is.Null);

        Assert.That(
            subscription.MessageHandlerTypes,
            Is.EquivalentTo(new[]
            {
                typeof(HelloHandler),
            }));
    }

    [Test]
    public void SubscribeType_HandlerTypeIsNull_ThrowsException()
    {
        // Arrange
        using var subscriber = this.CreateStandardMessageSubscriber();

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => subscriber.Subscribe(null!))!;

        // Assert
        Assert.That(ex.ParamName, Is.EqualTo("messageHandlerType"));
    }

    [Test]
    [TestCase(typeof(IInterfaceHandler))]
    [TestCase(typeof(StructHandler))]
    public void SubscribeType_HandlerTypeIsNotClass_ThrowsException(Type badHandlerType)
    {
        // Arrange
        using var subscriber = this.CreateStandardMessageSubscriber();

        // Act
        var ex = Assert.Throws<ArgumentException>(() => subscriber.Subscribe(badHandlerType))!;

        // Assert
        Assert.That(ex, Has.Message.StartWith("'messageHandlerType' must represent a class."));
        Assert.That(ex.ParamName, Is.EqualTo("messageHandlerType"));
    }

    [Test]
    public void SubscribeType_HandlerTypeIsAbstract_ThrowsException()
    {
        // Arrange
        using var subscriber = this.CreateStandardMessageSubscriber();

        // Act
        var ex = Assert.Throws<ArgumentException>(() => subscriber.Subscribe(typeof(AbstractHandler)))!;

        // Assert
        Assert.That(ex, Has.Message.StartWith("'messageHandlerType' cannot be abstract."));
        Assert.That(ex.ParamName, Is.EqualTo("messageHandlerType"));
    }

    [Test]
    public void SubscribeType_HandlerTypeDoesNotImplementInterface_ThrowsException()
    {
        // Arrange
        using var subscriber = this.CreateStandardMessageSubscriber();

        // Act
        var ex = Assert.Throws<ArgumentException>(() => subscriber.Subscribe(typeof(NoInterfaceHandler)))!;

        // Assert
        Assert.That(ex, Has.Message.StartWith("'messageHandlerType' must implement 'TauCode.Mq.IMessageHandler'."));
        Assert.That(ex.ParamName, Is.EqualTo("messageHandlerType"));
    }

    [Test]
    public void SubscribeType_HandlerTypeDoesNotImplementGenericInterface_ThrowsException()
    {
        // Arrange
        using var subscriber = this.CreateStandardMessageSubscriber();

        // Act
        var ex = Assert.Throws<ArgumentException>(() => subscriber.Subscribe(typeof(NotGenericInterfaceHandler)))!;

        // Assert
        Assert.That(ex, Has.Message.StartWith("'messageHandlerType' must implement 'TauCode.Mq.IMessageHandler<TMessage>' once."));
        Assert.That(ex.ParamName, Is.EqualTo("messageHandlerType"));
    }

    [Test]
    public void SubscribeType_HandlerTypeImplementsGenericInterfaceMoreThanOnce_ThrowsException()
    {
        // Arrange
        using var subscriber = this.CreateStandardMessageSubscriber();

        // Act
        var ex = Assert.Throws<ArgumentException>(() => subscriber.Subscribe(typeof(TwiceHandler)))!;

        // Assert
        Assert.That(ex, Has.Message.StartWith("'messageHandlerType' must implement 'TauCode.Mq.IMessageHandler<TMessage>' once."));
        Assert.That(ex.ParamName, Is.EqualTo("messageHandlerType"));
    }

    [Test]
    public void SubscribeType_HandlerTypeIsValidButAddedTwice_ThrowsException()
    {
        // Arrange
        using var subscriber = this.CreateStandardMessageSubscriber();

        subscriber.Subscribe(typeof(HelloHandler));

        // Act
        var ex = Assert.Throws<MqException>(() => subscriber.Subscribe(typeof(HelloHandler)))!;

        // Assert
        Assert.That(
            ex.Message,
            Is.EqualTo(
                $"Handler type '{typeof(HelloHandler).FullName}' already registered for message type '{typeof(HelloMessage).FullName}' (no topic)."));
    }

    [Test]
    [TestCase(typeof(HandlerWhichImplementsInterfaceMessage))]
    [TestCase(typeof(HandlerWhichImplementsAbstractMessage))]
    public void SubscribeType_MessageTypeIsAbstract_ThrowsException(Type badHandlerType)
    {
        // Arrange
        using var subscriber = this.CreateStandardMessageSubscriber();

        // Act
        var ex = Assert.Throws<ArgumentException>(() => subscriber.Subscribe(badHandlerType))!;

        // Assert

        var messageType = badHandlerType.BaseType!.GetGenericArguments().Single();

        Assert.That(ex.ParamName, Is.EqualTo("messageType"));
        Assert.That(ex.Message, Does.StartWith($"Cannot handle abstract message type '{messageType.FullName}'."));
    }

    private IMessageSubscriber CreateStandardMessageSubscriber()
    {
        var factory = new GoodContextFactory();
        IMessageSubscriber subscriber = new EasyNetQMessageSubscriber(factory, DefaultConnectionString, _logger)
        {
            Name = "my-sub",
        };

        return subscriber;
    }
}
