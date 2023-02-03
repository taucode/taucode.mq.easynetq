namespace TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

public class ThrowPropertyMessage : IMessage
{
    private string _badProperty = null!;

    public string? Topic { get; set; }
    public string? CorrelationId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string BadProperty
    {
        get
        {
            if (_badProperty == "bad")
            {
                throw new NotSupportedException("Property is bad!");
            }

            return _badProperty;
        }
        set => _badProperty = value;
    }
}