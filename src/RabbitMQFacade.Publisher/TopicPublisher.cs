using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQFacade;
using RabbitMQFacade.Connections;
using RabbitMQFacade.Serialization;

namespace RabbitMQFacade.Publisher
{
    public class TopicPublisher : RabbitMQClient
    {
       
        public TopicPublisher(string serverUri, string exchange, IConnectionPool connectionPool, ISerializerNegotiator serializerNegotiator, ILoggerProvider loggerProvider) 
            : base(serverUri, exchange, connectionPool, serializerNegotiator, loggerProvider) 
        {
            
            this.logger = loggerProvider.CreateLogger("TopicPublisher");
        }


        public Task Publish<T>(string routingKey, Message<T> message)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            try
            {
                IBasicProperties properties = this.channel.CreateBasicProperties();

                if (message.Headers == null)
                {
                    throw new ArgumentNullException("message.Headers");    
                }

                properties.ContentType = message.Headers.ContentType;
                properties.ContentEncoding = message.Headers.ContentEncoding.ToString();

               ISerializer serializer = this.SerializerNegotiator.Negotiate(properties);

                if (serializer == null)
                {
                    throw new ArgumentException("Not found compatible contentType nor content encoding on message to publish");
                }

                var bytes = serializer.Serialize(message);

                this.channel.BasicPublish(this.Exchange, routingKey, properties, bytes);
            }
            catch (Exception ex)
            {

                tcs.SetException(ex);
                tcs.SetCanceled();
                
            }
            finally
            {
                tcs.SetResult(true);
            }
            

            return tcs.Task;
        }
        
    }
}
