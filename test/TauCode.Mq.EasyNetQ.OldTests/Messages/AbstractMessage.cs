namespace TauCode.Mq.EasyNetQ.OldTests.Messages;

public abstract class AbstractMessage : IMessage
{
    public string? Topic { get; set; }
    public string? CorrelationId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public abstract int Age { get; set; }
}