namespace TauCode.Mq.EasyNetQ.Tests.Messages.Good;

public class ByeMessage : IMessage
{
    public string? Topic { get; set; }
    public string? CorrelationId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public string? Name { get; set; }
}