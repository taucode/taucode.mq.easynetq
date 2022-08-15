using EasyNetQ;
using Serilog;
using TauCode.Mq.Exceptions;
using TauCode.Working;

namespace TauCode.Mq.EasyNetQ;

public class EasyNetQMessagePublisher : MessagePublisherBase, IEasyNetQMessagePublisher
{
    #region Fields

    private string? _connectionString;
    private IBus? _bus;

    #endregion

    #region Constructors

    public EasyNetQMessagePublisher()
        : base(null)
    {
    }

    public EasyNetQMessagePublisher(ILogger? logger)
        : base(logger)
    {
    }

    public EasyNetQMessagePublisher(string? connectionString, ILogger? logger)
        : base(logger)
    {
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

        this.ContextLogger?.Verbose(
            "Inside method '{0:l}'. Bus is created.",
            nameof(InitImpl));
    }

    protected override void ShutdownImpl()
    {
        _bus!.Dispose();
        _bus = null;

        this.ContextLogger?.Verbose(
            "Inside method '{0:l}'. Bus is disposed.",
            nameof(ShutdownImpl));
    }

    protected override void PublishImpl(IMessage message)
    {
        if (message.Topic == null!)
        {
            _bus!.PubSub.Publish(message, message.GetType());
        }
        else
        {
            if (string.IsNullOrWhiteSpace(message.Topic))
            {
                throw new ArgumentException(
                    "Message topic can be null, but cannot be empty or white-space.",
                    nameof(message));
            }

            _bus!.PubSub.Publish(message, message.GetType(), message.Topic);
        }
    }

    #endregion

    #region IEasyNetQMessagePublisher Members

    public string? ConnectionString
    {
        get => _connectionString!;
        set
        {
            this.AllowIfStateIs($"set {nameof(ConnectionString)}", WorkerState.Stopped);

            // todo clean
            //if (this.State != WorkerState.Stopped)
            //{
            //    throw new InvalidOperationException($"Cannot set '{nameof(ConnectionString)}' while publisher is running.");
            //}

            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.Name);
            }

            _connectionString = value;
        }
    }

    #endregion
}