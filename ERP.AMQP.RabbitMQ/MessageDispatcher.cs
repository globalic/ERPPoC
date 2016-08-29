using ERP.AMQP.Core;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace ERP.AMQP.RabbitMQ
{
    public class MessageDispatcher<T> : IAMQPDispatcher<T>
    {
        public bool Enabled { get; set; }
        private delegate void ConsumeDelegate();
        string exchangeName_;
        string routingKey_;
        private ConnectionFactory connectionFactory_;
        private IConnection connection_;
        private IModel model_;


        private string queueName_;

        public MessageDispatcher(string exchangeName, string routingKey, ConnectionFactory conFactory)
        {
            exchangeName_ = exchangeName;
            routingKey_ = routingKey;
            connectionFactory_ = conFactory;
            connection_ = connectionFactory_.CreateConnection();
            model_ = connection_.CreateModel();
            model_.BasicQos(0, 1, false);
            model_ = connection_.CreateModel();
        }

        public void Send(T message, string routingKey)
        {
            //Setup properties
            var properties = model_.CreateBasicProperties();
            properties.Persistent=true;
            var serializedMessage = JsonConvert.SerializeObject(message);
            //Serialize
            byte[] messageBuffer = Encoding.Default.GetBytes(serializedMessage);

            //Send message
            model_.BasicPublish(exchangeName_, routingKey, properties, messageBuffer);
        }

        public void Dispose()
        {
            if (model_ != null)
                model_.Dispose();
            if (connection_ != null)
                connection_.Dispose();

            connectionFactory_ = null;

            GC.SuppressFinalize(this);
        }
    }
}
