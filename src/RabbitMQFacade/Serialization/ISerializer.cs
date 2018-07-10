using System.Text;
using RabbitMQ.Client;

namespace RabbitMQFacade.Serialization
{
    public interface ISerializer
    {
        byte[] Serialize<T>(Message<T> message);

        Message<T> DeSerialize<T>(byte[] bytes, IBasicProperties messageProperties);

        string ContentType { get; }

        string ContentEncoding { get; }


    }
}