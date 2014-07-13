using System;
using System.Text;
using RabbitMQ.Client.MessagePatterns;

namespace Server1
{
    public class RabbitConsumer2
    {
        private const string HstName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QName = "Q2";
        private const string ExchangeName = "";//Exchange Name by default is direct
        private Subscription _subscription = null;

        public void Start()
        {
            Console.WriteLine("\nListening to  RabbitMq Messaging Q2");
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                HostName = HstName,
                UserName = UserName,
                Password = Password
            };

            var connection = connectionFactory.CreateConnection();

            var model = connection.CreateModel();

            model.BasicQos(0, 1, false);



            _subscription = new Subscription(model, QName);

            var consumer = new ConsumeDelegate(Poll);
            consumer.Invoke();
        }

        private void Poll()
        {
            while (true)
            {
                var messageArgs = _subscription.Next();

                var message = Encoding.Default.GetString(messageArgs.Body);

                Console.WriteLine("Message recieved => " + message);

                _subscription.Ack(messageArgs);
            }
        }


        private delegate void ConsumeDelegate();
    }
}