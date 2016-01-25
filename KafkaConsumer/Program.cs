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
            Consume(getKafkaBroker(), getTopicName());
            
        }

        private static void Consume(string broker, string topic)
        {   
            var options = new KafkaOptions(new Uri(broker));
            var router = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions(topic, router));

            //Consume returns a blocking IEnumerable (ie: never ending stream)
            foreach (var message in consumer.Consume())
            {
                Console.WriteLine("Response: Partition {0},Offset {1} : {2}",
                    message.Meta.PartitionId, message.Meta.Offset, message.Value.ToUtf8String());
            }
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
