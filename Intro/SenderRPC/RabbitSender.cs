using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.Impl.v0_8;

namespace SenderRPC
{
    class RabbitSender
    {
        private string _reponseQ;
        private QueueingBasicConsumer _consumer;
        private IModel _consumermodel;
        private const string HstName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QName = "RPCQueue";
        private const string ExchangeName = "";//Exchange Name by default is direct
        public void Send()
        {
            Console.WriteLine("\nStarting RabbitMq Messaging");
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                HostName = HstName,
                UserName = UserName,
                Password = Password
            };

            var connection = connectionFactory.CreateConnection();

            _consumermodel = connection.CreateModel();

            //Create dynamic response Queue
            _reponseQ = _consumermodel.QueueDeclare().QueueName;
            _consumer = new QueueingBasicConsumer(_consumermodel);
            _consumermodel.BasicConsume(_reponseQ, true, _consumer);

          

            int messageCount = 0;
            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
                //Serialize
                byte[] message = Encoding.Default.GetBytes("Sabari has sent message" + messageCount);

                //send message to exchange
                var response = SendMessage(message, new TimeSpan(0, 0, 3, 0));

                Console.WriteLine("Message Sent -" + messageCount);
                Console.WriteLine("Message Response -" + response);

                messageCount++;

            }

            Console.WriteLine("\n Press any key to exit");

            Console.ReadLine();
        }

        private string SendMessage(byte[] message, TimeSpan timeSpan)
        {
            var correlationToken = Guid.NewGuid().ToString();

            var properties = _consumermodel.CreateBasicProperties();
            properties.ReplyTo = _reponseQ;
            properties.CorrelationId = correlationToken;

            var timeout = DateTime.Now + timeSpan;
            _consumermodel.BasicPublish("",QName,properties,message);

            while (DateTime.Now <= timeout)
            {
                var deliveryArgs = _consumer.Queue.Dequeue();

                if (deliveryArgs.BasicProperties != null &&
                    deliveryArgs.BasicProperties.CorrelationId == correlationToken)
                {
                    var response = Encoding.Default.GetString(deliveryArgs.Body);
                    return response;
                }
            }

            return "";


        }
    }
}
