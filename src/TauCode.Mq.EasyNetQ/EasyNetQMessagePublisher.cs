using EasyNetQ.NonGeneric;
using TauCode.Mq.Abstractions;
using TauCode.Mq.Exceptions;
using TauCode.Working;
using IBus = EasyNetQ.IBus;
using RabbitHutch = EasyNetQ.RabbitHutch;

namespace TauCode.Mq.EasyNetQ;

public class EasyNetQMessagePublisher : MessagePublisherBase, IEasyNetQMessagePublisher
{
    #region Fields

    private string _connectionString;
    private IBus _bus;

    #endregion

    #region Constructors

    public EasyNetQMessagePublisher()
    {
    }

    public EasyNetQMessagePublisher(string connectionString)
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

        _bus = RabbitHutch.CreateBus(this.ConnectionString);
    }

    protected override void ShutdownImpl()
    {
        _bus.Dispose();
        _bus = null;
    }

    protected override void PublishImpl(IMessage message)
    {
        if (message.Topic == null)
        {
            _bus.Publish(message.GetType(), message);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(message.Topic))
            {
                throw new ArgumentException("Message topic can be null, but cannot be empty or white-space.", nameof(message));
            }

            _bus.Publish(message.GetType(), message, message.Topic);
        }
    }

    #endregion

    #region IEasyNetQMessagePublisher Members

    public string ConnectionString
    {
        get => _connectionString;
        set
        {
            if (this.State != WorkerState.Stopped)
            {
                throw new MqException("Cannot set connection string while publisher is running.");
            }

            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.Name);
            }

            _connectionString = value;
        }
    }

    #endregion
}