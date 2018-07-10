using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using System.Linq;

namespace RabbitMQFacade.Serialization
{
    public class DefaultSerializerNegotiator : ISerializerNegotiator
    {

        ICollection<ISerializer> Serializers { get; set; }

        public DefaultSerializerNegotiator(ICollection<ISerializer>  serializers)
        {
            this.Serializers = serializers;
        }

        public ISerializer Negotiate(IBasicProperties properties)
        {
            return this.Serializers.Where((serializer) => !string.IsNullOrEmpty(properties.ContentType) && !string.IsNullOrEmpty(properties.ContentEncoding) && properties.ContentType.Equals(serializer.ContentType) && properties.ContentEncoding.Equals(serializer.ContentEncoding)).FirstOrDefault();
        }
    }
}
