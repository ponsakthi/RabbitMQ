using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace Server1
{
    class Program
    {
       

        private static void Main(string[] args)
        {
           new RabbitConsumer2().Start();
        }
    }
}
