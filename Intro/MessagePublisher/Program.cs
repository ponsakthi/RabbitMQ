using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace MessagePublisher
{
    class Program
    {
        private const string HstName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QName = "SampleQueue";
        private const string ExchangeName = "";//Exchange Name by default is direct
        static void Main(string[] args)
        {
            Console.WriteLine("\nStarting RabbitMq Messaging");
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                HostName = HstName,
                UserName = UserName,
                Password = Password
            };

            var connection = connectionFactory.CreateConnection();

            var model = connection.CreateModel();

            var properties = model.CreateBasicProperties();
            properties.SetPersistent(true);

            //Serialize
            byte[] message = Encoding.Default.GetBytes("Sabari has sent a message");

            //send message
            model.BasicPublish(ExchangeName,QName,properties,message);

            Console.WriteLine("Message Sent");

            Console.ReadLine();
        }
    }
}
