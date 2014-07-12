using System;
using RabbitMQ.Client;

namespace Intro
{
    class Program
    {
        private const string HstName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        static void Main(string[] args)
        {
            Console.WriteLine("\nStarting RabbitMq Q Creator");
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                HostName = HstName,
                UserName = UserName,
                Password = Password
            };

            var connection = connectionFactory.CreateConnection();

            var model = connection.CreateModel();

            model.QueueDeclare(
                "SabariQueue", true, false, false, null);

            model.ExchangeDeclare("SabariExchange", ExchangeType.Topic);

            model.QueueBind("SabariQueue","SabariExchange","cars");

            Console.WriteLine("message and Q created successfully!!!");

            Console.ReadKey();

        }
    }
}
