﻿namespace TauCode.Mq.EasyNetQ.OldTests.BadHandlers;

public class NonGenericHandler : IMessageHandler
{
    public void Handle(IMessage message)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}