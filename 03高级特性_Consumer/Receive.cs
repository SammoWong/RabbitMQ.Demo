using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace _03高级特性_Consumer
{
    class Receive
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var queue = "高级特性";
                    channel.QueueDeclare(queue, true, false, false, null);//定义一个支持持久化的消息队列
                    channel.BasicQos(0, 1, false);//在一个消费者还在处理消息且没响应消息之前，不要给他分发新的消息，而是将这条新的消息发送给下一个不那么忙碌的消费者

                    Console.WriteLine("准备接收消息：");
                    var consumer = new EventingBasicConsumer(channel);//构建消费者实例
                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body;
                        var message = Encoding.UTF8.GetString(body);
                        SimulationTask(message);
                        channel.BasicAck(e.DeliveryTag, false);//消息确认信号（手动消息确认）
                    };

                    channel.BasicConsume(queue, false, consumer);

                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// 模拟消息任务的处理过程
        /// </summary>
        /// <param name="message">消息</param>
        private static void SimulationTask(string message)
        {
            Console.WriteLine("接收的消息： {0}", message);
            int dots = message.Split('.').Length - 1;
            Thread.Sleep(dots * 1000);
            Console.WriteLine("接收的消息处理完成，现在时间为{0}！", DateTime.Now);
        }
    }
}
