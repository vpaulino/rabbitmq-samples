
# Dependencies
## Docker  image - Rabbitmq server
Execute docker image and run container to use this sample. In the near future there will be a docker compose file 

docker run -d -p 15672:15672 -p 5672:5672  --hostname local-rabbitmq --name local-rabbitmq rabbitmq:3-management
