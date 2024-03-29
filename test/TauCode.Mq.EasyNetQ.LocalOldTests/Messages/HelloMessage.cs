﻿namespace TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

public class HelloMessage : IMessage
{
    public HelloMessage()
    {
    }

    public HelloMessage(string name)
    {
        this.Name = name;
    }

    public string? Topic { get; set; }
    public string? CorrelationId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Name { get; set; }
    public int MillisecondsTimeout { get; set; } = 0;
}