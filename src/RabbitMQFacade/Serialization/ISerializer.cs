namespace RabbitMQFacade.Serialization
{
    public interface ISerializer
    {
        byte[] Serialize<T>(Message<T> message);

        Message<T> DeSerialize<T>(byte[] bytes);


    }
}