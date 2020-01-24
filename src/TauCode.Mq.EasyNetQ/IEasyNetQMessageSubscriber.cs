namespace TauCode.Mq.EasyNetQ
{
    public interface IEasyNetQMessageSubscriber : IMessageSubscriber
    {
        string ConnectionString { get; set; }
    }
}
