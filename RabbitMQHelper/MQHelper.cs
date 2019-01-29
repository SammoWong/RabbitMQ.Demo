using EasyNetQ;
using EasyNetQ.Topology;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQHelper
{
    public class MQHelper : MessageBusBuilder
    {
        public MQHelper(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// 接收消息（fanout）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="handler">回调处理</param>
        /// <param name="exChangeName">交换器名</param>
        /// <param name="queueName">队列名</param>
        /// <param name="routingKey">路由名</param>
        public void FanoutConsume<T>(Action<T> handler, string exChangeName = "fanout_exchange_default",
            string queueName = "fanout_queue_default", string routingKey = "") where T : class
        {
            var bus = CreateMessageBus();
            var advancedBus = bus.Advanced;
            var exchange = advancedBus.ExchangeDeclare(exChangeName, ExchangeType.Fanout);
            var queue = advancedBus.QueueDeclare(queueName);
            advancedBus.Bind(exchange, queue, routingKey);
            advancedBus.Consume(queue, registration =>
            {
                registration.Add<T>((message, info) =>
                {
                    handler(message.Body);
                });
            });
        }

        /// <summary>
        /// 发送消息（fanout）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="t">消息内容</param>
        /// <param name="exChangeName">交换器名</param>
        /// <param name="routingKey">路由名</param>
        public void FanoutPublish<T>(T t, string exChangeName = "fanout_exchange_default", string routingKey = "") where T : class
        {
            using (var bus = CreateMessageBus())
            {
                using (var advancedBus = bus.Advanced)
                {
                    var exchange = advancedBus.ExchangeDeclare(exChangeName, ExchangeType.Fanout);
                    advancedBus.Publish(exchange, routingKey, false, new Message<T>(t));
                }
            }
        }

        /// <summary>
        /// 发送消息（direct）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="t">消息内容</param>
        /// <param name="exChangeName">交换器名</param>
        /// <param name="routingKey">路由名</param>
        public void DirectPublish<T>(T t, string exChangeName = "direct_exchange_default",
            string routingKey = "direct_route_default") where T : class
        {
            using (var bus = CreateMessageBus())
            {
                using (var adbus = bus.Advanced)
                {
                    var exchange = adbus.ExchangeDeclare(exChangeName, ExchangeType.Direct);
                    adbus.Publish(exchange, routingKey, false, new Message<T>(t));
                }
            }
        }

        /// <summary>
        /// 接收消息（direct）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="handler">回调处理</param>
        /// <param name="exChangeName">交换器名</param>
        /// <param name="queueName">队列名</param>
        /// <param name="routingKey">路由名</param>
        public void DirectConsume<T>(Action<T> handler, string exChangeName = "direct_exchange_default",
            string queueName = "direct_queue_default", string routingKey = "direct_route_default") where T : class
        {
            var bus = CreateMessageBus();
            var advancedBus = bus.Advanced;
            var exchange = advancedBus.ExchangeDeclare(exChangeName, ExchangeType.Direct);
            var queue = advancedBus.QueueDeclare(queueName);
            advancedBus.Bind(exchange, queue, routingKey);
            advancedBus.Consume(queue, registration =>
            {
                registration.Add<T>((message, info) =>
                {
                    handler(message.Body);
                });
            });
        }

        /// <summary>
        /// 消息发送（direct）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="queue">队列名</param>
        /// <param name="message">消息内容</param>
        public void DirectSend<T>(string queue, T message) where T : class
        {
            using (var bus = CreateMessageBus())
            {
                bus.Send(queue, message);
            }
        }

        /// <summary>
        /// 消息接收（direct）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="queue">队列名</param>
        /// <param name="handler">回调处理</param>
        public void DirectReceive<T>(string queue, Action<T> handler) where T : class
        {
            var bus = CreateMessageBus();
            bus.Receive<T>(queue, handler);
        }

        /// <summary>
        /// 发布主题（topic）
        /// </summary>
        /// <typeparam name="T">主题类型</typeparam>
        /// <param name="topic">主题名称</param>
        /// <param name="message">主题内容</param>
        public void TopicPublish<T>(string topic, T message) where T : class
        {
            using (var bus = CreateMessageBus())
            {
                bus.Publish(message, topic);
            }
        }

        /// <summary>
        /// 订阅主题
        /// </summary>
        /// <typeparam name="T">主题类型</typeparam>
        /// <param name="subscriptionId">订阅者ID</param>
        /// <param name="handler">回调处理</param>
        /// <param name="topics">订阅主题集合</param>
        public void TopicSubscribe<T>(string subscriptionId, Action<T> handler, params string[] topics) where T : class
        {
            var bus = CreateMessageBus();
            bus.Subscribe(subscriptionId, handler, (config) =>
            {
                foreach (var item in topics)
                {
                    config.WithTopic(item);
                }
            });
        }

        /// <summary>
        /// 发布主题（topic）
        /// </summary>
        /// <typeparam name="T">主题类型</typeparam>
        /// <param name="topic">主题名称</param>
        /// <param name="t">主题内容</param>
        /// <param name="exChangeName">交换器名</param>
        public void TopicPublish<T>(string topic, T t, string exChangeName = "topic_exchange_default") where T : class
        {
            using (var bus = CreateMessageBus())
            {
                using (var advancedBus = bus.Advanced)
                {
                    var exchange = advancedBus.ExchangeDeclare(exChangeName, ExchangeType.Topic);
                    advancedBus.Publish(exchange, topic, false, new Message<T>(t));
                }
            }
        }

        /// <summary>
        /// 订阅主题
        /// </summary>
        /// <typeparam name="T">主题类型</typeparam>
        /// <param name="handler">回调处理</param>
        /// <param name="exChangeName">交换器名</param>
        /// <param name="subscriptionId">订阅者ID</param>
        /// <param name="topics">主题名集合</param>
        public void TopicConsume<T>(Action<T> handler, string exChangeName = "topic_exchange_default", string subscriptionId = "topic_subid",
            params string[] topics) where T : class
        {
            var bus = CreateMessageBus();
            var advancedBus = bus.Advanced;
            var exchange = advancedBus.ExchangeDeclare(exChangeName, ExchangeType.Topic);
            var queue = advancedBus.QueueDeclare(subscriptionId);
            foreach (var item in topics)
            {
                advancedBus.Bind(exchange, queue, item);
            }
            advancedBus.Consume(queue, registration =>
            {
                registration.Add<T>((message, info) =>
                {
                    handler(message.Body);
                });
            });
        }
    }
}
