﻿using TauCode.Mq.EasyNetQ.LocalOldTests.Messages;

namespace TauCode.Mq.EasyNetQ.LocalOldTests.BadHandlers;

public struct StructHandler : IMessageHandler<HelloMessage>
{
    public void Handle(HelloMessage message)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(HelloMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public void Handle(IMessage message)
    {
        throw new NotSupportedException();
    }

    public Task HandleAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}