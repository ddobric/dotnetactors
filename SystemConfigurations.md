# System Configurations

To run the dotnetactor service some basic configurations are required to setup that are:

1. Before starting the service a service bus connection string needs to be setup as an environment variable as shown in the fig below.

![image](https://user-images.githubusercontent.com/28738233/119394639-b3c33180-bcd2-11eb-84f6-bc81698b0b7a.png)

2. To run the service below following command line arguments needs to pass :

--SystemName=HelloCluster
-- RequestMsgTopic=actorsystem/actortopic
-- RequestMsgQueue=actorsystem/actorqueue
--ActorSystemName=actorsystem
--SubscriptionName=default

3. Thats it hit enter, the service is up and running
4. To run the service on multiple nodes, execute the same command on multiple terminals


