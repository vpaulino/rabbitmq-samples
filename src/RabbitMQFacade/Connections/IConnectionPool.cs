using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQFacade.Connections
{
    public interface IConnectionPool
    {
        IModel GetOrCreateChannel(string serverUri);
    }
}
