using System;

namespace ERP.AMQP.Core
{
    public interface IAMQPDispatcher<T> : IDisposable
    {
        bool Enabled { get; set; }
        void Send(T message, string routingKey);
    }
}
