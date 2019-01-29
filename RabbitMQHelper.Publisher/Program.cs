using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQHelper.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Publish();
            Console.ReadKey();
        }

        public static void Publish()
        {
            //MQHelperFactory.Default().FanoutPublish("这是一条新消息");
            //Console.WriteLine("这是一条新消息");
            //Thread.Sleep(10 * 1000);
            //MQHelperFactory.Default().FanoutPublish("这是二条新消息");
            //Console.WriteLine("这是二条新消息");

            //MQHelperFactory.Default().DirectPublish(DateTime.Now.ToLongDateString());

            //MQHelperFactory.Default().DirectSend("default", DateTime.Now.ToLongDateString());

            //MQHelperFactory.Default().TopicPublish("defaultTopic", DateTime.Now.ToLongDateString());

            MQHelperFactory.Default().TopicPublish("defaultTopic", DateTime.Now.ToLongDateString(), exChangeName: "topic_exchange_default");

        }
    }
}
