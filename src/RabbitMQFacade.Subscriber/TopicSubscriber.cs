using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQFacade.Connections;
using RabbitMQFacade.Serialization;

namespace RabbitMQFacade.Subscriber
{
    public class TopicSubscriber<T>: RabbitMQClient,  IBasicConsumer
    {
      
         

        public TopicSubscriber(string serverUri, string exchange, IConnectionPool connectionPool, ISerializer serializer, ILoggerProvider loggerProvider)
             : base(serverUri, exchange, connectionPool, serializer, loggerProvider)
        {
           
            

            this.logger = loggerProvider.CreateLogger("TopicPublisher");
        }
         


        public Task Subscribe(string queueName, bool autoAck = true)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            bool result = true;

            try
            {
                channelControl.Wait();
                var consume = this.channel.BasicConsume(queueName, autoAck, this);
                channelControl.Release();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, this.channel);
                result = false;
            }
            finally
            {
                tcs.SetResult(result);
            }


            return tcs.Task;
             

        }

        #region interface implementation
        public void HandleBasicCancel(string consumerTag)
        {
            this.logger.LogInformation($"TopicSubscriber.HandleBasicCancel - consumerTag: {consumerTag}");
        }

        public void HandleBasicCancelOk(string consumerTag)
        {
            this.logger.LogInformation($"TopicSubscriber.HandleBasicCancelOk - consumerTag: {consumerTag}");
        }

        public void HandleBasicConsumeOk(string consumerTag)
        {
            this.logger.LogInformation($"TopicSubscriber.HandleBasicConsumeOk - consumerTag: {consumerTag}");
        }

        public event EventHandler<Message<T>> OnMessageReceived;
        public event EventHandler<ConsumerEventArgs> ConsumerCancelled;

        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            this.logger.LogInformation($"TopicSubscriber.HandleBasicConsumeOk - {{ consumerTag: \"{consumerTag}\",deliveryTag : \"{deliveryTag}\",redelivered : \"{redelivered}\",exchange : \"{exchange}\",routingKey : \"{routingKey}\"   }} ");
            Message<T>  result = this.Serializer.DeSerialize<T>(body);

            OnMessageReceived?.Invoke(this, result);
            
        }
        public void HandleModelShutdown(object model, ShutdownEventArgs reason)
        {
            this.logger.LogInformation($"TopicSubscriber.HandleBasicConsumeOk - ShutdownEventArgs.Cause: {reason.Cause}, ShutdownEventArgs.ReplyText: {reason.ReplyText}, ChannelNumber: {(model as IModel).ChannelNumber} IsClosed : {(model as IModel).IsClosed},  CloseReason : {(model as IModel).CloseReason}");
        }


        #endregion
    }
}
