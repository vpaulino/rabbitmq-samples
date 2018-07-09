using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using RabbitMQFacade;
using RabbitMQFacade.Connections;
using RabbitMQFacade.Publisher;
using RabbitMQFacade.Serialization.Json;
using RabbitMQFacade.Subscriber;

namespace ClientApp
{
    public class SubscriberClient : IClient
    {

        TopicSubscriber<string> subscriber;
        private string routingKey;
        public SubscriberClient(string server, string exchange, string queueName, IConnectionPool connPool)
        {
            ILoggerProvider loggerProvider = new ConsoleLoggerProvider(new ConsoleLoggerSettings());
            subscriber = new TopicSubscriber<string>(server, exchange, connPool, new NewtonSoftSerialization(), loggerProvider);
            subscriber.OnMessageReceived += Subscriber_OnMessageReceived;
        }

        private void Subscriber_OnMessageReceived(object sender, Message<string> e)
        {
            Console.WriteLine($"{{ CorrelationId: {e.CorrelationId}, Created: {e.Created}, ContentType: {e.ContentType}  Payload: {e.Body}  }}");
        }

        public void Start()
        {
            subscriber.Connect();
            subscriber.Subscribe("metrics.process");
             
        }

        public void Stop()
        {
            subscriber.Disconnect();
             
        }



    }
}
