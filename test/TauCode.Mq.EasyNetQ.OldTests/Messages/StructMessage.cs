namespace TauCode.Mq.EasyNetQ.OldTests.Messages;

public struct StructMessage : IMessage
{
    public string? Topic { get; set; }
    public string? CorrelationId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Category { get; set; }
}