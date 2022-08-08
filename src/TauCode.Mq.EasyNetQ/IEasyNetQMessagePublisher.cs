namespace TauCode.Mq.EasyNetQ;

public interface IEasyNetQMessagePublisher : IMessagePublisher
{
    string ConnectionString { get; set; }
}