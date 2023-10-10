using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;

namespace RabbitMq.Manager.Core
{
    public class Consumer
    {
        private Action<string> OnRecieved;
        private IModel _channel;

        private string _queueName;

        public Consumer(Action<string> onRecieved, string queueName, IModel channel)
        {
            OnRecieved = onRecieved;
            _channel = channel;
            _queueName = queueName;
        }

        protected void Job()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                OnRecieved.Invoke(message);
                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName, consumer: consumer, autoAck: false);
        }

        public void Run()
        {
            Task.Run(Job);
        }
    }
}
