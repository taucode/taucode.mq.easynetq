using System;
using TauCode.Mq.Abstractions;

namespace TauCode.Mq.EasyNetQ.IntegrationTests.Messages
{
    public class ThrowPropertyMessage : IMessage
    {
        private string _badProperty;

        public string Topic { get; set; }
        public string CorrelationId { get; set; }
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
}
