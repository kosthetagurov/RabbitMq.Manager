using RabbitMQ.Client;

namespace RabbitMq.Manager.Core
{
    public class Queue
    {
        public string Name { get; set; }

        private Consumer _consumer;

        public Queue(string name, IModel channel, Action<string> onRecieved)
        {
            Name = name;
            _consumer = new Consumer(onRecieved, Name, channel);

            channel.QueueDeclare(queue: Name,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
        }

        public void RunConsume()
        {
            _consumer.Run();
        }
    }
}
