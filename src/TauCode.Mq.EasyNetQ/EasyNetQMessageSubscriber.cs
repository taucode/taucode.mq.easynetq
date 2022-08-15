using EasyNetQ;
using Serilog;
using TauCode.Mq.Exceptions;
using TauCode.Working;

using ITauMessage = TauCode.Mq.IMessage;

namespace TauCode.Mq.EasyNetQ;

public class EasyNetQMessageSubscriber : MessageSubscriberBase, IEasyNetQMessageSubscriber
{
    #region Fields

    private string? _connectionString;
    private IBus? _bus;

    #endregion

    #region Constructor

    public EasyNetQMessageSubscriber(IMessageHandlerContextFactory contextFactory)
        : base(contextFactory, null)
    {
    }

    public EasyNetQMessageSubscriber(
        IMessageHandlerContextFactory contextFactory,
        ILogger? logger)
        : base(contextFactory, logger)
    {
    }

    public EasyNetQMessageSubscriber(
        IMessageHandlerContextFactory contextFactory,
        string? connectionString,
        ILogger? logger)
        : base(contextFactory, logger)
    {
        ArgumentNullException.ThrowIfNull(contextFactory);

        this.ConnectionString = connectionString;
    }

    #endregion

    #region Overridden

    protected override void InitImpl()
    {
        if (string.IsNullOrEmpty(this.ConnectionString))
        {
            throw new MqException("Cannot start: connection string is null or empty.");
        }

        _bus = RabbitHutch.CreateBus(
            this.ConnectionString,
            x => x.EnableSerilogLogging(this.OriginalLogger));
    }

    protected override void ShutdownImpl()
    {
        _bus?.Dispose();  // todo: will wait until IMessageHandler.HandleAsync returns?
        _bus = null;
    }

    protected override IDisposable SubscribeImpl(ISubscriptionRequest subscriptionRequest)
    {
        var subscriptionId = Guid.NewGuid().ToString();

        IDisposable handle;

        if (subscriptionRequest.Topic == null)
        {
            Task EasyNetQHandler(object messageObject, Type type, CancellationToken cancellationToken) =>
                subscriptionRequest.AsyncHandler((ITauMessage)messageObject, cancellationToken);

            handle = _bus!.PubSub.Subscribe(
                subscriptionId,
                subscriptionRequest.MessageType,
                EasyNetQHandler,
                configuration => configuration.WithAutoDelete());
        }
        else
        {
            Task EasyNetQHandler(object messageObject, Type type, CancellationToken cancellationToken) =>
                subscriptionRequest.AsyncHandler((ITauMessage)messageObject, cancellationToken);

            handle = _bus!.PubSub.Subscribe(
                subscriptionId,
                subscriptionRequest.MessageType,
                EasyNetQHandler,
                configuration => configuration
                    .WithTopic(subscriptionRequest.Topic)
                    .WithAutoDelete());
        }

        return handle;
    }

    #endregion

    #region IEasyNetQMessageSubscriber Members

    public string? ConnectionString
    {
        get => _connectionString!;
        set
        {
            this.AllowIfStateIs($"set {nameof(ConnectionString)}", WorkerState.Stopped);


            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.Name);
            }

            _connectionString = value;
        }
    }

    #endregion
}