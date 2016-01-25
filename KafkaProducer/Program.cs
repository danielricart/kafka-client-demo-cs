using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace ApacheKafkaDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Produce();
                System.Threading.Thread.Sleep(3000);
            } while (true);
        }

        private static void Produce()
        {
            var broker = "http://localhost:9092";
            var topic = "MyTopic";

            var options = new KafkaOptions(new Uri(broker));
            var router = new BrokerRouter(options);
            var client = new Producer(router);

            var current_datetime =DateTime.Now;
            var key = current_datetime.Second.ToString();
            var events = new[] { new Message("Hello World " + current_datetime.ToString(), key) };
            client.SendMessageAsync(topic, events).Wait(1500);
            Console.WriteLine("Produced: Key: {0}. Message: {1}", key, events[0].Value.ToUtf8String());

            using (client) { }
        }
    }
}
