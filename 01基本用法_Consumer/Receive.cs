using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace _01基本用法_Consumer
{
    class Receive
    {
        static void Main(string[] args)
        {
            //实例化连接工厂
            var factory = new ConnectionFactory { HostName = "localhost" };
            //建立连接
            using(var connection = factory.CreateConnection())
            {
                //创建信道
                using(var channel = connection.CreateModel())
                {
                    var queue = "01基本用法";
                    channel.QueueDeclare(queue, false, false, false, null);
                    Console.WriteLine("准备接收消息：");
                    //构建消费者实例
                    var consumer = new EventingBasicConsumer(channel);
                    //消息接收事件
                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("接收到的消息： {0}", message);

                        //消息确认信号（手动消息确认）
                        //channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
                    };
                    //开启消费者与通道、队列关联
                    //autoAck:true；自动进行消息确认，当消费端接收到消息后，就自动发送ack信号，不管消息是否正确处理完毕
                    //autoAck:false；关闭自动消息确认，通过调用BasicAck方法手动进行消息确认
                    channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
                    Console.ReadLine();
                }
            }

        }
    }
}
