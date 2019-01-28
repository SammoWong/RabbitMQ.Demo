using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace _02主题发布订阅_Consumer
{
    class Receive
    {
        static void Main(string[] args)
        {
            //实例化连接工厂
            var factory = new ConnectionFactory { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var exchange = "主题发布订阅";
                    channel.ExchangeDeclare(exchange, ExchangeType.Topic);//创建类型为topic的交换机
                    var queueName = channel.QueueDeclare().QueueName;//获取连接通道所使用的队列
                    Console.Write("请输入准备监听的消息主题格式，格式如'*.rabbit'或者'info.*'或者'info.warning.error'等：");
                    while (true)
                    {
                        var bindingKey = Console.ReadLine();
                        channel.QueueBind(queueName, exchange, bindingKey);//队列绑定到交换机
                        Console.WriteLine("准备接收消息");
                        var consumer = new EventingBasicConsumer(channel);//构建消费者示例

                        consumer.Received += (sender, e) =>
                        {
                            var routingKey = e.RoutingKey;
                            var body = e.Body;
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine("接收到的消息： {0}:{1}", routingKey, message);

                            //消息确认信号（手动消息确认）
                            channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
                        };
                        channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    }
                }
            }

        }
    }
}
