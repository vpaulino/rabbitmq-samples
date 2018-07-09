using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQFacade.Connections;

namespace RabbitMQFacade.Publisher
{

    
    public class MemoryConnectionPool : IConnectionPool
    {
        private IConnectionFactory connectionFactory = new ConnectionFactory();
        private ConcurrentDictionary<string, IConnection> connectionPool = new ConcurrentDictionary<string, IConnection>();
        SemaphoreSlim createConnectionSem = new SemaphoreSlim(1);
        private ILogger logger;

        public MemoryConnectionPool(ILoggerProvider loggerProvider)
        {
            logger = loggerProvider.CreateLogger("MemoryConnectionPool");
        }
            
        public IModel GetOrCreateChannel(string serverUri)
        {

            createConnectionSem.Wait();

            IConnection connection = connectionPool.GetOrAdd(serverUri, CreateConnection);
            IModel result = connection.CreateModel();

            createConnectionSem.Release();

            return result;
        }

        private IConnection CreateConnection(string serverUri)
        {
            var conn = connectionFactory.CreateConnection(new List<AmqpTcpEndpoint>() { new AmqpTcpEndpoint(new Uri(serverUri)) });

            conn.ConnectionShutdown += Conn_ConnectionShutdown;
            conn.ConnectionRecoveryError += Conn_ConnectionRecoveryError;
            conn.RecoverySucceeded += Conn_RecoverySucceeded;

            return conn;
        }

        private void Conn_RecoverySucceeded(object sender, EventArgs e)
        {
            logger.LogInformation($"Conn_RecoverySucceeded - {sender.ToString()} : {e.ToString()}");
        }

        private void Conn_ConnectionRecoveryError(object sender, ConnectionRecoveryErrorEventArgs e)
        {
            logger.LogInformation($"Conn_ConnectionRecoveryError - {sender.ToString()} : {e.ToString()}");
        }

        private void Conn_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            logger.LogInformation($"Conn_ConnectionShutdown - {sender.ToString()} : {e.ToString()}");
        }
    }
}
