using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    class Program
    {
        private const string HstName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QName = "SendAndRecieve";
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

            int messageCount = 0;
            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
                //Serialize
                byte[] message = Encoding.Default.GetBytes("Sabari has sent message" + messageCount);

                //send message
                model.BasicPublish(ExchangeName, QName, properties, message);

                Console.WriteLine("Message Sent -" + messageCount);
             
                messageCount++;

            }

            Console.WriteLine("\n Press any key to exit");

            Console.ReadLine();
        }
    }
}
