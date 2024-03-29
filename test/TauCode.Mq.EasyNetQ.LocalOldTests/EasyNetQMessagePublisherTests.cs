﻿using System.Text;
using EasyNetQ;
using Newtonsoft.Json;
using NUnit.Framework;
using Serilog;
using TauCode.IO;
using TauCode.Mq.EasyNetQ.LocalOldTests.Messages;
using TauCode.Mq.Exceptions;
using TauCode.Working;

namespace TauCode.Mq.EasyNetQ.LocalOldTests;

[TestFixture]
public class EasyNetQMessagePublisherTests
{
    private const string DefaultConnectionString = "host=localhost";

    private ILogger _logger;
    private StringWriterWithEncoding _writer;

    [SetUp]
    public void SetUp()
    {
        _writer = new StringWriterWithEncoding(Encoding.UTF8);
        _logger = new LoggerConfiguration()
            .WriteTo.TextWriter(_writer)
            .MinimumLevel.Verbose()
            .CreateLogger();
    }

    #region ctor

    [Test]
    public void Constructor_NoArguments_RunsOk()
    {
        // Arrange

        // Act
        using IEasyNetQMessagePublisher messagePublisher = new EasyNetQMessagePublisher(_logger);

        // Assert
        Assert.That(messagePublisher.ConnectionString, Is.Null);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("host=some-host")]
    public void Constructor_ConnectionString_RunsOk(string connectionString)
    {
        // Arrange

        // Act
        using IEasyNetQMessagePublisher messagePublisher = new EasyNetQMessagePublisher(connectionString, _logger);

        // Assert
        Assert.That(messagePublisher.ConnectionString, Is.EqualTo(connectionString));
    }

    #endregion

    #region ConnectionString

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("host=some-host")]
    public void ConnectionString_NotStarted_CanBeSet(string connectionString)
    {
        // Arrange
        using IEasyNetQMessagePublisher messagePublisher = new EasyNetQMessagePublisher(_logger);

        // Act
        messagePublisher.ConnectionString = connectionString;

        // Assert
        Assert.That(messagePublisher.ConnectionString, Is.EqualTo(connectionString));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("host=localhost")]
    public void ConnectionString_StoppedThenStarted_CanBeSet(string connectionString)
    {
        // Arrange
        using IEasyNetQMessagePublisher messagePublisher = new EasyNetQMessagePublisher("host=localhost", _logger);
        messagePublisher.Start();
        messagePublisher.Stop();

        // Act
        messagePublisher.ConnectionString = connectionString;

        // Assert
        Assert.That(messagePublisher.ConnectionString, Is.EqualTo(connectionString));
    }

    [Test]
    public void ConnectionString_StartedThenSet_ThrowsException()
    {
        // Arrange
        using IEasyNetQMessagePublisher messagePublisher = new EasyNetQMessagePublisher(_logger)
        {
            ConnectionString = "host=localhost",
            Name = "my-pub",
        };

        messagePublisher.Start();

        // Act
        var connectionString = messagePublisher.ConnectionString;

        var ex = Assert.Throws<InvalidOperationException>(() => messagePublisher.ConnectionString = "host=127.0.0.1");

        // Assert
        Assert.That(connectionString, Is.EqualTo("host=localhost"));
        Assert.That(ex.Message, Is.EqualTo("Cannot perform operation 'set ConnectionString'. Worker state is 'Running'. Worker name is 'my-pub'."));
    }

    [Test]
    public void ConnectionString_DisposedThenSet_ThrowsException()
    {
        // Arrange
        using IEasyNetQMessagePublisher messagePublisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-publisher",
        };

        // Act
        messagePublisher.Dispose();
        var connectionString = messagePublisher.ConnectionString;

        var ex = Assert.Throws<ObjectDisposedException>(() => messagePublisher.ConnectionString = "host=127.0.0.1");

        // Assert
        Assert.That(connectionString, Is.EqualTo("host=localhost"));
        Assert.That(ex!.ObjectName, Is.EqualTo("my-publisher"));
    }

    #endregion

    #region Publish(IMessage)

    [Test]
    public async Task Publish_ValidStateAndArguments_PublishesAndProperSubscriberHandles()
    {
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();

        string name = null;
        string nameWithTopic = null;

        string name1;
        string nameWithTopic1;

        string name2;
        string nameWithTopic2;

        using var bus = RabbitHutch.CreateBus("host=localhost");

        var subId1 = Guid.NewGuid().ToString();
        using var sub1 = bus.PubSub.Subscribe<HelloMessage>(
            subId1,
            message => name = message.Name,
            configuration => configuration
                .WithAutoDelete());

        var subId2 = Guid.NewGuid().ToString();
        using var sub2 = bus.PubSub.Subscribe<HelloMessage>(
            subId2,
            message => nameWithTopic = message.Name,
            configuration => configuration
                .WithTopic("some-topic")
                .WithAutoDelete());

        // Act
        publisher.Publish(
            new HelloMessage
            {
                Name = "mia",
                Topic = "some-topic",
            });

        await Task.Delay(1500);

        name1 = name;
        nameWithTopic1 = nameWithTopic;

        publisher.Publish(new HelloMessage
        {
            Name = "deserea",
        });

        await Task.Delay(1500);

        name2 = name;
        nameWithTopic2 = nameWithTopic;

        // Assert
        Assert.That(name1, Is.EqualTo("mia"));
        Assert.That(nameWithTopic1, Is.EqualTo("mia"));

        Assert.That(name2, Is.EqualTo("deserea"));
        Assert.That(nameWithTopic2, Is.EqualTo("mia"));
    }

    [Test]
    public void Publish_ArgumentIsNull_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => publisher.Publish(null));

        // Assert
        Assert.That(ex.ParamName, Is.EqualTo("message"));
    }

    [Test]
    public void Publish_ArgumentIsNotClassNoTopic_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();

        // Act
        var ex = Assert.Throws<ArgumentException>(() => publisher.Publish(new StructMessage()));

        // Assert
        Assert.That(
            ex,
            Has.Message.StartWith(
                $"Cannot publish instance of '{typeof(StructMessage).FullName}'. Message type must be a class."));
        Assert.That(ex.ParamName, Is.EqualTo("message"));
    }

    [Test]
    public void Publish_ArgumentIsNotClassWithTopic_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();

        // Act
        var ex = Assert.Throws<ArgumentException>(() => publisher.Publish(new StructMessage
        {
            Topic = "some-topic",
        }));

        // Assert
        Assert.That(ex,
            Has.Message.StartWith(
                $"Cannot publish instance of '{typeof(StructMessage).FullName}'. Message type must be a class."));
        Assert.That(ex.ParamName, Is.EqualTo("message"));
    }

    [Test]
    public void Publish_ArgumentPropertyThrows_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();

        // Act
        var ex = Assert.Throws<JsonSerializationException>(() =>
        {
            publisher.Publish(new ThrowPropertyMessage
            {
                BadProperty = "bad",
            });
        });

        // Assert
        Assert.That(ex.InnerException, Is.TypeOf<NotSupportedException>());
        Assert.That(ex.InnerException, Has.Message.EqualTo("Property is bad!"));
    }

    [Test]
    public void Publish_NoTopicNotStarted_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();
        publisher.Name = "my-pub";

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => publisher.Publish(new HelloMessage()));

        // Assert
        Assert.That(ex, Has.Message.EqualTo("Cannot perform operation 'Publish'. Worker state is 'Stopped'. Worker name is 'my-pub'."));
    }

    [Test]
    public void Publish_WithTopicNotStarted_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();
        publisher.Name = "my-pub";

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => publisher.Publish(
            new HelloMessage
            {
                Topic = "some-topic",
            }));

        // Assert
        Assert.That(ex, Has.Message.EqualTo("Cannot perform operation 'Publish'. Worker state is 'Stopped'. Worker name is 'my-pub'."));
    }

    [Test]
    public void Publish_NoTopicDisposed_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();
        publisher.Name = "my-publisher";

        publisher.Dispose();

        // Act
        var ex = Assert.Throws<ObjectDisposedException>(() => publisher.Publish(new HelloMessage()));

        // Assert
        Assert.That(ex.ObjectName, Is.EqualTo("my-publisher"));
    }

    [Test]
    public void Publish_WithTopicDisposed_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();
        publisher.Name = "my-publisher";

        publisher.Dispose();

        // Act
        var ex = Assert.Throws<ObjectDisposedException>(() => publisher.Publish(new HelloMessage
        {
            Topic = "my-topic",
        }));

        // Assert
        Assert.That(ex.ObjectName, Is.EqualTo("my-publisher"));
    }

    #endregion

    #region Name

    [Test]
    public void Name_Disposed_CanOnlyBeRead()
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "name1",
        };

        // Act
        publisher.Dispose();

        var name = publisher.Name;
        var ex = Assert.Throws<ObjectDisposedException>(() => publisher.Name = "name2")!;

        // Assert
        Assert.That(name, Is.EqualTo("name1"));
        Assert.That(publisher.Name, Is.EqualTo("name1"));
        Assert.That(ex.ObjectName, Is.EqualTo("name1"));
    }

    #endregion

    #region State

    [Test]
    public void State_JustCreated_EqualsToStopped()
    {
        // Arrange

        // Act
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger)
        {
            Name = "my-publisher"
        };

        // Assert
        Assert.That(publisher.State, Is.EqualTo(WorkerState.Stopped));
    }

    [Test]
    public void State_Started_EqualsToRunning()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        // Act
        publisher.Start();

        // Assert
        Assert.That(publisher.State, Is.EqualTo(WorkerState.Running));
    }

    [Test]
    public void State_Stopped_EqualsToStopped()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();

        // Act
        publisher.Stop();

        // Assert
        Assert.That(publisher.State, Is.EqualTo(WorkerState.Stopped));
    }

    [Test]
    public void State_DisposedJustAfterCreation_EqualsToStopped()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        // Act
        publisher.Dispose();

        // Assert
        Assert.That(publisher.State, Is.EqualTo(WorkerState.Stopped));
    }

    [Test]
    public void State_DisposedAfterStarted_EqualsToStopped()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();

        // Act
        publisher.Dispose();

        // Assert
        Assert.That(publisher.State, Is.EqualTo(WorkerState.Stopped));
    }

    [Test]
    public void State_DisposedAfterStopped_EqualsToStopped()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();
        publisher.Stop();

        // Act
        publisher.Dispose();

        // Assert
        Assert.That(publisher.State, Is.EqualTo(WorkerState.Stopped));
    }

    [Test]
    public void State_DisposedAfterDisposed_EqualsToStopped()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();
        publisher.Stop();
        publisher.Dispose();

        // Act
        publisher.Dispose();

        // Assert
        Assert.That(publisher.State, Is.EqualTo(WorkerState.Stopped));
    }

    #endregion

    #region IsDisposed

    [Test]
    public void IsDisposed_JustCreated_EqualsToFalse()
    {
        // Arrange

        // Act
        using var publisher = new EasyNetQMessagePublisher("host=localhost", _logger);

        // Assert
        Assert.That(publisher.IsDisposed, Is.False);
    }

    [Test]
    public void IsDisposed_Started_EqualsToFalse()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        // Act
        publisher.Start();

        // Assert
        Assert.That(publisher.IsDisposed, Is.False);
    }

    [Test]
    public void IsDisposed_Stopped_EqualsToFalse()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();

        // Act
        publisher.Stop();

        // Assert
        Assert.That(publisher.IsDisposed, Is.False);
    }

    [Test]
    public void IsDisposed_DisposedJustAfterCreation_EqualsToTrue()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        // Act
        publisher.Dispose();

        // Assert
        Assert.That(publisher.IsDisposed, Is.True);
    }

    [Test]
    public void IsDisposed_DisposedAfterStarted_EqualsToTrue()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();

        // Act
        publisher.Dispose();

        // Assert
        Assert.That(publisher.IsDisposed, Is.True);
    }

    [Test]
    public void IsDisposed_DisposedAfterStopped_EqualsToTrue()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();
        publisher.Stop();

        // Act
        publisher.Dispose();

        // Assert
        Assert.That(publisher.IsDisposed, Is.True);
    }

    [Test]
    public void IsDisposed_DisposedAfterDisposed_EqualsToTrue()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Dispose();

        // Act
        publisher.Dispose();

        // Assert
        Assert.That(publisher.IsDisposed, Is.True);
    }

    #endregion

    #region Start()

    [Test]
    public void Start_JustCreated_Starts()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        // Act
        publisher.Start();

        // Assert
        Assert.Pass();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void Start_ConnectionStringIsNullOrEmpty_ThrowsException(string badConnectionString)
    {
        // Arrange
        using var publisher = new EasyNetQMessagePublisher(badConnectionString, _logger);

        // Act
        var ex = Assert.Throws<MqException>(() => publisher.Start());

        // Assert
        Assert.That(ex, Has.Message.EqualTo("Cannot start: connection string is null or empty."));
    }

    [Test]
    public void Start_Started_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher(null, "my-publisher");

        publisher.Start();

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => publisher.Start());

        // Assert
        Assert.That(ex, Has.Message.EqualTo("Cannot perform operation 'Start'. Worker state is 'Running'. Worker name is 'my-publisher'."));
    }

    [Test]
    public void Start_Stopped_Starts()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();


        publisher.Start();
        publisher.Stop();

        // Act
        publisher.Start();

        // Assert
        Assert.Pass();
    }

    [Test]
    public void Start_Disposed_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();
        publisher.Name = "my-publisher";

        publisher.Dispose();

        // Act
        var ex = Assert.Throws<ObjectDisposedException>(() => publisher.Start());

        // Assert
        Assert.That(ex.ObjectName, Is.EqualTo("my-publisher"));
    }

    #endregion

    #region Stop()

    [Test]
    public void Stop_JustCreated_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher(null, "my-publisher");

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => publisher.Stop());

        // Assert
        Assert.That(ex, Has.Message.EqualTo("Cannot perform operation 'Stop'. Worker state is 'Stopped'. Worker name is 'my-publisher'."));
    }

    [Test]
    public void Stop_Started_Stops()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();

        // Act
        publisher.Stop();

        // Assert
        Assert.Pass();
    }

    [Test]
    public void Stop_Stopped_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher(null, "my-publisher");

        publisher.Start();
        publisher.Stop();

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => publisher.Stop());

        // Assert
        Assert.That(ex, Has.Message.EqualTo("Cannot perform operation 'Stop'. Worker state is 'Stopped'. Worker name is 'my-publisher'."));
    }

    [Test]
    public void Stop_Disposed_ThrowsException()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher(null, "my-publisher");

        publisher.Dispose();

        // Act
        var ex = Assert.Throws<ObjectDisposedException>(() => publisher.Stop());

        // Assert
        Assert.That(ex.ObjectName, Is.EqualTo("my-publisher"));
    }

    #endregion

    #region Dispose()

    [Test]
    public void Dispose_JustCreated_Disposes()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        // Act
        publisher.Dispose();

        // Assert
        Assert.Pass("Test passed.");
    }

    [Test]
    public void Dispose_Started_Disposes()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();

        // Act
        publisher.Dispose();

        // Assert
        Assert.Pass("Test passed.");
    }

    [Test]
    public void Dispose_Stopped_Disposes()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Start();
        publisher.Stop();

        // Act
        publisher.Dispose();

        // Assert
        Assert.Pass("Test passed.");
    }

    [Test]
    public void Disposes_Disposed_DoesNothing()
    {
        // Arrange
        using var publisher = this.CreateMessagePublisher();

        publisher.Dispose();

        // Act
        publisher.Dispose();

        // Assert
        Assert.Pass("Test passed.");
    }

    #endregion

    #region Private

    private IMessagePublisher CreateMessagePublisher(string? connectionString = null, string? name = null)
    {
        connectionString ??= DefaultConnectionString;

        var messagePublisher = new EasyNetQMessagePublisher(connectionString, _logger)
        {
            Name = name,
        };

        return messagePublisher;
    }

    #endregion
}