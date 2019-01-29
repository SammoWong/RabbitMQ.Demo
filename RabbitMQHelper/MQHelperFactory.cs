using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQHelper
{
    public class MQHelperFactory
    {
        public static MQHelper Default() => new MQHelper("host=192.168.0.168:5672;virtualHost=TestQueue;username=sammo;password=1qaz2wsx");
    }
}
