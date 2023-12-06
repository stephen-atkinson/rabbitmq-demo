SET RabbitMqServerPath="C:\Program Files\RabbitMQ Server\rabbitmq_server-3.12.7\sbin"

CD /D %RabbitMqServerPath%

set RABBITMQ_NODE_PORT=5673
set RABBITMQ_NODENAME=rabbit1
set RABBITMQ_SERVICE_NAME=rabbit1
set RABBITMQ_SERVER_START_ARGS=-rabbitmq_management listener [{port,15673}]
start rabbitmq-server
set RABBITMQ_NODE_PORT=5674
set RABBITMQ_NODENAME=rabbit2
set RABBITMQ_SERVICE_NAME=rabbit2
set RABBITMQ_SERVER_START_ARGS=-rabbitmq_management listener [{port,15674}]
start rabbitmq-server
set RABBITMQ_NODE_PORT=5675
set RABBITMQ_NODENAME=rabbit3
set RABBITMQ_SERVICE_NAME=rabbit3
set RABBITMQ_SERVER_START_ARGS=-rabbitmq_management listener [{port,15675}]
start rabbitmq-server