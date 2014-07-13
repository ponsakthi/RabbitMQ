using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Reciever
{
    class Program
    {
        private const string HstName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QName = "SendAndRecieve";
        private const string ExchangeName = "";//Exchange Name by default is direct

        private static void Main(string[] args)
        {
            Console.WriteLine("\nListening to  RabbitMq Messaging");
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                HostName = HstName,
                UserName = UserName,
                Password = Password
            };

            var connection = connectionFactory.CreateConnection();

            var model = connection.CreateModel();

            model.BasicQos(0,1,false);

            var consumer = new QueueingBasicConsumer(model);

            model.BasicConsume(QName, false, consumer);

            while (true)
            {
                var messageArgs = consumer.Queue.Dequeue();

                var serializedMessage = Encoding.Default.GetString(messageArgs.Body);

                Console.WriteLine("Message recieved -> {0}",serializedMessage);

                model.BasicAck(messageArgs.DeliveryTag,false);
            }


        }
    }
}
