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
       
        public TopicPublisher(string serverUri, string exchange, IConnectionPool connectionPool, ISerializer serializer, ILoggerProvider loggerProvider) 
            : base(serverUri, exchange, connectionPool, serializer, loggerProvider) 
        {
            
            this.logger = loggerProvider.CreateLogger("TopicPublisher");
        }


        public Task Publish<T>(string routingKey, Message<T> message)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            try
            {
                var bytes = Serializer.Serialize(message);

                this.channel.BasicPublish(this.Exchange, routingKey, null, bytes);
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
