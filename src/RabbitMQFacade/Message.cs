using System;
using System.Collections.Generic;

namespace RabbitMQFacade
{
    public class Message<T>
    {
        public MessageHeaders Headers { get; set; }
         
        public Message()
        {
            this.Created = DateTime.UtcNow;
        }

        public Message(T body, MessageHeaders headers) : this()
        {
            this.Body = body;
            this.Headers = headers;
        }

        

        public DateTime Created { get; set; }
         
        public T Body { get; set; }
    }
}