SET RabbitMqServerPath="C:\Program Files\RabbitMQ Server\rabbitmq_server-3.12.7\sbin"

CD /D %RabbitMqServerPath%

call rabbitmqctl -n rabbit2 stop_app
call rabbitmqctl -n rabbit2 join_cluster rabbit1@STEPHEN-PC-01
call rabbitmqctl -n rabbit2 start_app

call rabbitmqctl -n rabbit3 stop_app
call rabbitmqctl -n rabbit3 join_cluster rabbit1@STEPHEN-PC-01
call rabbitmqctl -n rabbit3 start_app

pause