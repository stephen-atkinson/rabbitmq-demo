SET RabbitMqServerPath="C:\Program Files\RabbitMQ Server\rabbitmq_server-3.12.7\sbin"

CD /D %RabbitMqServerPath%

set RABBITMQ_NODE_PORT=5674
set RABBITMQ_NODENAME=rabbit2
set RABBITMQ_SERVICE_NAME=rabbit2
set RABBITMQ_SERVER_START_ARGS=-rabbitmq_management listener [{port,15674}]
start rabbitmq-server