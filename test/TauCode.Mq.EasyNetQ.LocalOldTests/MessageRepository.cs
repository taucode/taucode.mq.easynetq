﻿namespace TauCode.Mq.EasyNetQ.LocalOldTests;

// todo: don't need it, indeed. log is enough.
public class MessageRepository
{
    public static MessageRepository Instance = new MessageRepository();

    private readonly List<IMessage> _messages;
    private readonly object _lock;

    private MessageRepository()
    {
        _messages = new List<IMessage>();
        _lock = new object();
    }

    public void Clear()
    {
        lock (_lock)
        {
            _messages.Clear();
        }
    }

    public void Add(IMessage message)
    {
        lock (_lock)
        {
            _messages.Add(message);
        }
    }

    public IReadOnlyList<IMessage> GetMessages()
    {
        lock (_lock)
        {
            return _messages.ToList();
        }
    }
}