using System;
using System.Collections.Generic;
using System.Configuration;
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
                Produce(getKafkaBroker(), getTopicName());
                System.Threading.Thread.Sleep(3000);
            } while (true);
        }

        private static void Produce(string broker, string topic)
        {
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

        private static string getKafkaBroker()
        {
            string KafkaBroker = string.Empty;
            var KafkaBrokerKeyName = "KafkaBroker";

            if (!ConfigurationManager.AppSettings.AllKeys.Contains(KafkaBrokerKeyName))
            {
                KafkaBroker = "http://localhost:9092";
            }
            else
            {
                KafkaBroker = ConfigurationManager.AppSettings[KafkaBrokerKeyName];
            }
            return KafkaBroker;
        }
        private static string getTopicName()
        {
            string TopicName = string.Empty;
            var TopicNameKeyName = "Topic";

            if (!ConfigurationManager.AppSettings.AllKeys.Contains(TopicNameKeyName))
            {
                throw new Exception("Key \"" + TopicNameKeyName + "\" not found in Config file -> configuration/AppSettings");
            }
            else
            {
                TopicName = ConfigurationManager.AppSettings[TopicNameKeyName];
            }
            return TopicName;
        }
    }
}
