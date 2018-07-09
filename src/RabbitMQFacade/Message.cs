using System;
using System.Collections.Generic;

namespace RabbitMQFacade
{
    public class Message<T>
    {

        public string ContentType { get; set; }

        public Message()
        {
            
            this.Headers = new Dictionary<string, object>();
            this.Created = DateTime.UtcNow;
        }

        public Message(T body)
        {
            this.Body = body;
            this.Headers = new Dictionary<string, object>();
            this.Created = DateTime.UtcNow;
        }

        public Message(Guid correlationId, T body) 
            : this(body)
        {
            this.CorrelationId = correlationId;
        }

        public Guid? CorrelationId { get; set; }
        public DateTime Created { get; set; }

        public Dictionary<string, object> Headers
        {
            get; private set;
        }

        public T Body { get; set; }
    }
}