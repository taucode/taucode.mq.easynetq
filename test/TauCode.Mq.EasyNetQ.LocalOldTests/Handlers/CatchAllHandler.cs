﻿namespace TauCode.Mq.EasyNetQ.LocalOldTests.Handlers;

// todo: use this one.

public class CatchAllHandler : MessageHandlerBase<IMessage>
{
    protected override Task HandleAsyncImpl(IMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}