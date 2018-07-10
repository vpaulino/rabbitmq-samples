using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQFacade.Connections;
using RabbitMQFacade.Serialization;

namespace RabbitMQFacade
{
    public class RabbitMQClient
    {

        public string ServerUri { get; private set; }

        protected IModel channel;
        public string Exchange { get; private set; }
        public ISerializerNegotiator SerializerNegotiator { get; }
        public IConnectionPool connectionPool { get; }

        public ILogger logger { get; set; }

        public IModel Model => throw new NotImplementedException();

        protected SemaphoreSlim channelControl = new SemaphoreSlim(1);
        
        public RabbitMQClient(string serverUri, string exchange, IConnectionPool connectionPool, ISerializerNegotiator serializerNegotiator, ILoggerProvider loggerProvider)
        {
            this.ServerUri = serverUri;
            this.Exchange = exchange;
            this.SerializerNegotiator = serializerNegotiator;
            this.connectionPool = connectionPool;

            this.logger = loggerProvider.CreateLogger("TopicPublisher");
        }

        public virtual Task<bool> Connect()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            bool result = true;
            try
            {
                this.channel = this.connectionPool.GetOrCreateChannel(this.ServerUri);

                this.channel.ModelShutdown += Channel_ModelShutdown;

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, this.ServerUri, this.channel);
                result = false;
            }
            finally
            {
                tcs.SetResult(result);
            }


            return tcs.Task;


        }

        public virtual Task<bool> Disconnect()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            bool result = true;

            try
            {
                channelControl.Wait();

                if (!this.channel.IsClosed)
                {
                    this.channel.Close();
                    this.channel.Dispose();

                }

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


        protected virtual void Channel_ModelShutdown(object sender, ShutdownEventArgs e)
        {
            this.logger.LogInformation(new EventId(1, "Channel_ModelShutdown"), "Channel_ModelShutdown fired ", sender, e);
            //this.logger.Info($"TopicPublisher.Channel_ModelShutdown; Sender: {sender.GetType().ToString()} -  Cause: {e.Cause}, ReplyCode: {e.ReplyCode}, ReplyText: {e.ReplyText} ");
        }


       


    }
}
