using RabbitMQ.Client;
using System;
using System.Text;

namespace _01基本用法_Publisher
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
                    var queue = "01基本用法";
                    //定义队列
                    channel.QueueDeclare(queue, false, false, false, null);

                    while (true)
                    {
                        Console.Write("请输入要发送的消息：");
                        var message = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(message);
                        //发送数据包
                        channel.BasicPublish("", queue, null, body);
                        Console.WriteLine("已发送的消息： {0}", message);
                    }
                }
            }
        }
    }
}
