using RabbitMQ.Client;

using System.Text;

namespace RabbitMq.Manager.Core
{
    public class Publisher
    {
        IModel _channel;

        public Publisher(IModel channel)
        {
            _channel = channel;
        }

        public void Publish(string exchange, string routingKey, string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            _channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: bytes);
        }
    }
}
