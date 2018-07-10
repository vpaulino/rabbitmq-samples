using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQFacade.Serialization
{
    public interface ISerializerNegotiator
    {
      

        ISerializer Negotiate(IBasicProperties messageProperties);
        
    }
}
