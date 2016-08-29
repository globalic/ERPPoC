using ERP.AMQP.Core;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Text;

namespace ERP.AMQP.RabbitMQ
{
    public class MessageConsumer<T> : IAMPQConsumer<T>
    {
        public event EventHandler<RecieveMessageArgs<T>> OnReceiveMessage;

        public bool Enabled { get; set; }
        private delegate void ConsumeDelegate();

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private Subscription _subscription;
        private string _queueName;

        public MessageConsumer(string queueName, ConnectionFactory conFactory)
        {
            _queueName = queueName;
            _connectionFactory = conFactory;
            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();
            _model.BasicQos(0, 1, false);
        }

        public void Start()
        {
            _subscription = new Subscription(_model, _queueName, false);

            var consumer = new ConsumeDelegate(Poll);
            consumer.Invoke();
        }

        private void Poll()
        {
            while (Enabled)
            {
                //Get next message
                var deliveryArgs = _subscription.Next();
                //Deserialize message
                var message = Encoding.Default.GetString(deliveryArgs.Body);
                var dsMessage = JsonConvert.DeserializeObject<T>(message);
                OnReceiveMessage.Invoke(this, new RecieveMessageArgs<T> { Data = dsMessage });
                //Acknowledge message is processed
                _subscription.Ack(deliveryArgs);
            }
        }

        public void Dispose()
        {
            if (_model != null)
                _model.Dispose();
            if (_connection != null)
                _connection.Dispose();

            _connectionFactory = null;

            GC.SuppressFinalize(this);
        }


        public void Stop()
        {
            _subscription.Close();
            Dispose();
        }
    }
}
