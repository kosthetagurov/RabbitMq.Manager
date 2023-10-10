using RabbitMq.Manager.Core;

using RabbitMQ.Client;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            var channel = connectionFactory.CreateConnection().CreateModel();

            var exchange = new Exchange("users.portal", RabbitMq.Manager.Core.ExchangeType.Topic, channel);
            exchange
                .BindQueue(new Queue("users.main", channel, Console.WriteLine), "users.*")
                .BindQueue(new Queue("users.test", channel, Console.WriteLine), "users.*")
                .BindQueue(new Queue("users.test.new", channel, Console.WriteLine), "users.*")
                .BindQueue(new Queue("users.email", channel, Console.WriteLine))
                .RunConsume();

            var publisher = new Publisher(channel);
            publisher.Publish("users.portal", "users.*", "users message");

            Console.ReadKey();
        }
    }
}