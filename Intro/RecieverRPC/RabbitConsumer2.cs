using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace Server1
{
    public class RabbitConsumer2
    {
        private const string HstName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QName = "RPCQueue";
        private const string ExchangeName = "";//Exchange Name by default is direct
        private Subscription _subscription = null;
        private IModel _model;
        private ConsumeDelegate consumer;
        private QueueingBasicConsumer _consumer;

        public void Start()
        {
            Console.WriteLine("\nListening to  RabbitMq Messaging");
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                HostName = HstName,
                UserName = UserName,
                Password = Password
            };

            var connection = connectionFactory.CreateConnection();

            _model = connection.CreateModel();

            _model.BasicQos(0, 1, false);


            _consumer = new QueueingBasicConsumer(_model);

            _model.BasicConsume(QName, false, _consumer);

             Poll();
        }

        private void Poll()
        {
            while (true)
            {
                var messageArgs = _consumer.Queue.Dequeue();

                var message = Encoding.Default.GetString(messageArgs.Body);

                Console.WriteLine("Message recieved => " + message);

                //sending response
                var replyProp = _model.CreateBasicProperties();
                replyProp.CorrelationId = messageArgs.BasicProperties.CorrelationId;
                byte[] replyBytes = Encoding.Default.GetBytes("Sending response for " + message);

                _model.BasicPublish("",messageArgs.BasicProperties.ReplyTo,replyProp,replyBytes);

                _model.BasicAck(messageArgs.DeliveryTag,false);
            }
        }


        private delegate void ConsumeDelegate();
    }
}