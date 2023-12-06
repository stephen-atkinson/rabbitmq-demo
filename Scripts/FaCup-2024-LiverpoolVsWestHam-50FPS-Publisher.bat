SET Path="..\RabbitMqDemo.Publisher\bin\Debug\net8.0"

CD /D %Path%

RabbitMqDemo.Publisher.exe --publisher:exchange main.positioning --publisher:routingkey facup.2024.anfield.liverpoolvswestham.realtime.centreofmass --publisher:fps 50