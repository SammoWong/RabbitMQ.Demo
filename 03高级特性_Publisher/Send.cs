using RabbitMQ.Client;
using System;
using System.Text;

namespace _03高级特性_Publisher
{
    class Send
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    var queue = "高级特性";
                    channel.QueueDeclare(queue, true, false, false, null);//定义一个支持持久化的消息队列
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;//1:不持久化，2：持久化
                    Console.WriteLine("请注意：演示耗时较长的消息时，可通过发送带有‘.’的内容去模拟，每个‘.’加1秒！");
                    while (true)
                    {
                        Console.WriteLine("请输入要发送的信息：");
                        var message = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish("", queue, properties, body);
                        Console.WriteLine("已发送的消息： {0}", message);
                    }
                }
            }
        }
    }
}
