namespace TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

public class DecayingMessage : IMessage
{
    public static bool IsPropertyDecayed { get; set; }
    public static bool IsCtorDecayed { get; set; }

    private string _decayedProperty = null!;

    public DecayingMessage()
    {
        if (IsCtorDecayed)
        {
            throw new Exception("Alas Ctor Decayed!");
        }
    }

    public string? Topic { get; set; }
    public string? CorrelationId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public string DecayedProperty
    {
        get => _decayedProperty;
        set
        {
            if (IsPropertyDecayed)
            {
                throw new Exception("Alas Property Decayed!");
            }

            _decayedProperty = value;
        }
    }
}