using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQHelper
{
    public class MessageBusBuilder
    {
        private string ConnectionString { get; set; }

        public MessageBusBuilder(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IBus CreateMessageBus()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new Exception("ConnectionString not exist");

            return RabbitHutch.CreateBus(ConnectionString);
        }
    }
}
