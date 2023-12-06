
## Contents
### Applications

- Producer - For generating and sending messages to the exchange
- Consumer - For reading the messages from a queue
- Manager - For creating the topology (exchanges, queues and bindings)

### Scripts

RabbitMQ -
- **Create Nodes** - Spins up 3 brokers \
AMQP Ports - 5673, 5674 and 5675\
Management Portal URLs - \
Broker 1 - http://localhost:15673 \
Broker 2 - http://localhost:15674 \
Broker 3 - http://localhost:15675 \

- **Create Cluster** - Connects the 3 brokers together to form a cluster
- **Remove Cluster** - Disconnects the 3 brokers from the cluster
- **Reset Nodes** - Wipes all the data (exchanges, queues, bindings, users, policies, cluster, etc.)

Application
- **Run Manager** - Runs the manager application (no additional settings are added in this script)
- **Premier League-2024-ChelseaVsArsenal-25FPS-Publisher** - Publishes messages at 25 FPS with the routing key - premierleague.2024.stamfordbridge.chelseavsarsenal.realtime.centreofmass
- **Premier League-2024-LeedsVsManchesterCity-25FPS-Publisher** - Publishes messages at 25 FPS with the routing key - premierleague.2024.ellandroad.leedsvsmanchestercity.realtime.centreofmass
- **FaCup-2024-LiverpoolVsWestHam-50FPS-Publisher** - Publishes messages at 50 FPS with the routing key - facup.2024.anfield.liverpoolvswestham.realtime.centreofmass
- **Premier League-2024-LeedsVsManchesterCity-Consumer** - Consumes messages from the queue - rabbitmqdemo.ellandroad.realtime.centreofmass

## How to run

1. Install RabbitMQ from [Downloading and Installing RabbitMQ — RabbitMQ](https://www.rabbitmq.com/download.html)
2. Check the RabbitMQ directory in the RabbitMQ scripts is correct for your installation
3. Run "Create Nodes.bat"
4. Visit the management portal URLs and login with the default credentials (guest/guest)
5. Run "Create Cluster.bat" and check the nodes are connected on the "overview" page
6. Run "Run Manager.bat" then check the exchanges and queues have been created
7. Run "Premier League-2024-LeedsVsManchesterCity-25FPS-Publisher.bat" and check the exchanges and queues have messages
8. Run "Premier League-2024-LeedsVsManchesterCity-Consumer", the *rabbitmqdemo.ellandroad.realtime.centreofmass* queue should now be empty
9. Run the remaining publisher.bat files for more messages