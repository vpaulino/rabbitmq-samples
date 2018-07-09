

# Introduction

This solutions consists on a Facade over RabbitMQ C# client that expose two clients types to interact with rabbitmq. TopicPublisher and TopicSubscriber. 

This two types have dependencies to be used such as ILoggerProvider from Microsoft.Extensions.Logging and an implementation of ISerializable. 

To see the usage of thesse two entities please check Hosts/ClientApp/ folder where you can find a PublishClient and a SubscriberClient. 

To connections reuse please consider the approach showned here in the program.cs file where the ICOnnectionPoll implementation is shared between those two Rabbit CLient types.



# Environment
## Docker  image - Rabbitmq server
Execute docker image and run container to use this sample. In the near future there will be a docker compose file 

docker run -d -p 15672:15672 -p 5672:5672  --hostname local-rabbitmq --name local-rabbitmq rabbitmq:3-management
