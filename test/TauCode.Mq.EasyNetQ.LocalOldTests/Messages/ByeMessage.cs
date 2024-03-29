﻿namespace TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

public class ByeMessage : IMessage
{
    public ByeMessage()
    {
    }

    public ByeMessage(string nickname)
    {
        this.Nickname = nickname;
    }

    public string? Topic { get; set; }
    public string? CorrelationId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Nickname { get; set; } = null!;
    public int MillisecondsTimeout { get; set; } = 0;
}