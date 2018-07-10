using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQFacade
{
    public class MessageHeaders
    {
        public MessagePersistence DeliveryMode { get; set; }

        public string ContentType { get; set; }

        public Guid CorrelationId { get; internal set; }
        public string ContentEncoding { get; set; }
    }
}
