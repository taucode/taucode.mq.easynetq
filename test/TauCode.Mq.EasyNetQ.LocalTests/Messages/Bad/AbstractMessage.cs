namespace TauCode.Mq.EasyNetQ.LocalTests.Messages.Bad;

public abstract class AbstractMessage : IMessage
{
    public string? Topic { get; set; }
    public string? CorrelationId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}