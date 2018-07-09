using System;
using System.IO;
using Newtonsoft.Json;

namespace RabbitMQFacade.Serialization.Json
{
    public class NewtonSoftSerialization : ISerializer
    {


        public Message<T> DeSerialize<T>(byte[] bytes)
        {

            try
            {
                string instanceJson = System.Text.Encoding.UTF8.GetString(bytes);
                var instance = JsonConvert.DeserializeObject<Message<T>>(instanceJson);

                return instance;
            }
            catch (Exception ex)
            {

                return null;
            }
        
            
        }

        public byte[] Serialize<T>(Message<T> message)
        {
            
            var instanceJson = JsonConvert.SerializeObject(message);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(instanceJson);
            
            return bytes;
             
        }
    }
}
