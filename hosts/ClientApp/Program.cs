using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging.Console;
using RabbitMQFacade.Publisher;
using RabbitMQFacade.Serialization;
using RabbitMQFacade.Serialization.Json;

namespace ClientApp
{
    class Program
    {


        static Dictionary<string, Func<Tuple<string,string,string>,IClient>> clients = new Dictionary<string, Func<Tuple<string, string, string>, IClient>>();
        static void Main(string[] args)
        {
            var argumentsParsed = ParseArguments(args);

            MemoryConnectionPool connectionPool = new MemoryConnectionPool(new ConsoleLoggerProvider(new ConsoleLoggerSettings()));
            DefaultSerializerNegotiator negotiator = new DefaultSerializerNegotiator(new List<ISerializer>() { new NewtonSoftSerialization() });

            clients.Add("1", (arguments) => new PublisherClient(arguments.Item1, arguments.Item2, arguments.Item3, connectionPool, negotiator));
            clients.Add("2", (arguments) => new SubscriberClient(arguments.Item1, arguments.Item2, arguments.Item3, connectionPool, negotiator));

            Console.WriteLine("Choose what type of client is this:");
            Console.WriteLine("1 - Publisher");
            Console.WriteLine("2 - Subscriber");
            Console.WriteLine("3 - Booth");
            var readed = Console.ReadLine();

            Func<Tuple<string, string, string>, IClient> clientHandler = null;

            IClient client = null;

            if (clients.TryGetValue(readed, out clientHandler))
            {
                client = clientHandler(new Tuple<string, string, string>(argumentsParsed.server, argumentsParsed.exchange, argumentsParsed.routing));
                client.Start();
            }
            else
            {
                Func<Tuple<string, string, string>, IClient> subHandler = null;
                Func<Tuple<string, string, string>, IClient> pubHandler = null;
                clients.TryGetValue("1", out pubHandler);
                clients.TryGetValue("2", out subHandler);

                var subClient = subHandler(new Tuple<string, string, string>(argumentsParsed.server, argumentsParsed.exchange, argumentsParsed.routing));
                subClient.Start();
                Thread.Sleep(1000);
                var pub = pubHandler(new Tuple<string, string, string>(argumentsParsed.server, argumentsParsed.exchange, argumentsParsed.routing));
                pub.Start();
            }



            Console.WriteLine("To Stop press any key:");
            readed = Console.ReadLine();

            client.Stop();

        }

        private static (string server, string exchange, string routing) ParseArguments(string[] args)
        {
            return (args[1], args[2], args[3]);
        }
    }
}
