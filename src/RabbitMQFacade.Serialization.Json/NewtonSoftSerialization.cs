using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQFacade.Serialization.Json
{
    public class NewtonSoftSerialization : ISerializer
    {
        public string ContentType { get { return MediaTypes.ApplicationJson; }  }

        public string ContentEncoding { get { return System.Text.Encoding.UTF8.HeaderName; } }

        public Message<T> DeSerialize<T>(byte[] bytes, IBasicProperties messageProperties)
        {

            try
            {
                
                Encoding encoding = Encoding.GetEncoding(messageProperties.ContentEncoding);
                string instanceJson = encoding?.GetString(bytes);
                if (string.IsNullOrEmpty(instanceJson))
                {
                    throw new ArgumentOutOfRangeException("IBasicProperties.ContentEncoding");
                }

                if (!messageProperties.IsContentTypePresent())
                {
                    throw new ArgumentNullException("IBasicProperties.ContentType");
                }

                if (!messageProperties.ContentType.Equals(this.ContentType))
                {
                    throw new ArgumentException("Message ContentType do not match with selected Serialization ContentType","IBasicProperties.ContentType");
                }

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
