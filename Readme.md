

# Introduction

This solution consists on a set of Facades over [RabbitMQ C# client library](https://www.rabbitmq.com/dotnet-api-guide.html).
This facades abstracts the boilerplate code that enables comunication with rabbimtmq broker in diferent comunication patterns, such as:

 - [x]  Publisher/Subscriber
 - [ ]  RPC

This two types have dependencies to be used such as ILoggerProvider from Microsoft.Extensions.Logging and an implementation of ISerializable. 

To see the usage of thesse two entities please check Hosts/ClientApp/ folder where you can find a PublishClient and a SubscriberClient. 

To connections reuse please consider the approach showned here in the program.cs file where the ICOnnectionPoll implementation is shared between those two Rabbit CLient types.



# Environment
## Docker  image - Rabbitmq server
To test ./src/hosts/ClientApp executable consider installing one of the rabbitmq docker images available [here](https://hub.docker.com/_/rabbitmq/) and expose the ports that you want to use:

* 5672 - amqp protocol comunication
* 15672 - rabbitMQ management site 

docker run -d -p 15672:15672 -p 5672:5672  --hostname local-rabbitmq --name local-rabbitmq rabbitmq:3-management
