SET RabbitMqServerPath="C:\Program Files\RabbitMQ Server\rabbitmq_server-3.12.7\sbin"

CD /D %RabbitMqServerPath%

call rabbitmqctl -n rabbit2 stop_app
call rabbitmqctl -n rabbit2 reset
call rabbitmqctl -n rabbit2 start_app

call rabbitmqctl -n rabbit3 stop_app
call rabbitmqctl -n rabbit3 reset
call rabbitmqctl -n rabbit3 start_app

pause