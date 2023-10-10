using RabbitMQ.Client;

namespace RabbitMq.Manager.Core
{
    public class Exchange
    {
        public string Name { get; }
        public ExchangeType Type { get; }
        public bool Durable { get; }
        public bool AutoDelete { get; }

        private IModel _channel;

        private List<Queue> _queues;

        public Exchange(string name, ExchangeType type, IModel channel, bool durable = false, bool autoDelete = false)
        {
            Name = name;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;

            _channel = channel;

            _queues = new List<Queue>();

            _channel.ExchangeDeclare(Name, Type.ToString().ToLower(), Durable, AutoDelete);
        }

        public Exchange BindQueue(Queue queue, string? routingKey = null)
        {            
            _channel.QueueBind(queue.Name, Name, routingKey ?? queue.Name);
            _queues.Add(queue);
            return this;
        }

        public void RunConsume()
        {
            foreach (var queue in _queues)
            {
                queue.RunConsume();
            }
        }
    }
}
