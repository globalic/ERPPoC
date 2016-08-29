using System;

namespace ERP.AMQP.Core
{
    public interface IAMPQConsumer<T> : IDisposable
    {
        event EventHandler<RecieveMessageArgs<T>> OnReceiveMessage;

        bool Enabled { get; set; }

        void Start();
        void Stop();
    }

}