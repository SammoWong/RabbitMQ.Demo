using RabbitMQ.Client;
using System;
using System.Text;

namespace _02主题发布订阅_Publisher
{
    class Send
    {
        static void Main(string[] args)
        {
            //实例化连接工厂
            var factory = new ConnectionFactory { HostName = "localhost" };
            //建立连接
            using (var connection = factory.CreateConnection())
            {
                //创建信道
                using (var channel = connection.CreateModel())
                {
                    var exchange = "主题发布订阅";
                    //创建类型为topic的交换机
                    channel.ExchangeDeclare(exchange, ExchangeType.Topic);

                    while (true)
                    {
                        Console.WriteLine("请输入要发送的消息，格式如'RoutingKey_Message'：");
                        var keyWithMessage = Console.ReadLine();
                        args = keyWithMessage.Split("_");
                        var routingKey = args.Length > 1 ? args[0] : "*.rabbit";
                        var message = args.Length > 1 ? args[1] : "Hello World";
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange, routingKey, null, body);
                        Console.WriteLine("已发送的消息： '{0}':'{1}'", routingKey, message);
                    }
                }
            }
        }
    }
}
