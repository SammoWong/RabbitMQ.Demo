using System;
using System.Threading.Tasks;

namespace RabbitMQHelper.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Consume();
            Console.ReadKey();
        }

        public static void Consume()
        {
            //MQHelperFactory.Default().FanoutConsume<string>((message) =>
            //{
            //    Console.WriteLine($"已接收 => {message}");
            //});

            //MQHelperFactory.Default().DirectConsume<string>((str) => {
            //    Console.WriteLine(str);
            //});

            //MQHelperFactory.Default().DirectReceive<string>("default", (str) =>
            //{
            //    Console.WriteLine(str);
            //});

            //MQHelperFactory.Default().TopicSubscribe<string>("defaultId", (str) =>
            //{
            //    Console.WriteLine(str);
            //}, "*.rabbit");

            MQHelperFactory.Default().TopicConsume<string>((str) =>
            {
                Console.WriteLine(str);
            });
        }
    }
}
