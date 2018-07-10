using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using RabbitMQFacade;
using RabbitMQFacade.Connections;
using RabbitMQFacade.Publisher;
using RabbitMQFacade.Serialization;
using RabbitMQFacade.Serialization.Json;

namespace ClientApp
{
    public class PublisherClient : IClient
    {

        TopicPublisher publisher;
        private string routingKey;
        public PublisherClient(string server, string exchange, string routingKey, IConnectionPool connPool, ISerializerNegotiator serializerNegotiator)
        {

            ILoggerProvider loggerProvider = new ConsoleLoggerProvider(new ConsoleLoggerSettings());
            publisher = new TopicPublisher(server, exchange, connPool, serializerNegotiator, loggerProvider);
            this.routingKey = routingKey;
        }
        Timer timer;
        public void Start()
        {
            publisher.Connect();


            timer = new Timer();
            timer.Interval = new TimeSpan(0, 0, 2).TotalMilliseconds;
            timer.Elapsed += (obj, args) =>
              {
                  var currentProcess = System.Diagnostics.Process.GetCurrentProcess();

                  publisher.Publish<string>(routingKey, new Message<string>($"VirtualMemorySize64:  {currentProcess.VirtualMemorySize64.ToString()}, Threads: {currentProcess.Threads.Count} ", new MessageHeaders() { ContentType = MediaTypes.ApplicationJson,  ContentEncoding = Encoding.UTF8.HeaderName }));

              };

            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
            timer.Dispose();
            publisher.Disconnect();
             
        }



    }
}
