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
            Consume();
        }

        private static void Consume()
        {
            var broker = "http://localhost:9092";
            var topic = "MyTopic";

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
    }
}
