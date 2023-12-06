SET Path="..\RabbitMqDemo.Publisher\bin\Debug\net8.0"

CD /D %Path%

RabbitMqDemo.Publisher.exe --publisher:exchange main.positioning --publisher:routingkey premierleague.2024.stamfordbridge.chelseavsarsenal.realtime.centreofmass --publisher:fps 25