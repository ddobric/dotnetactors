# System Configurations

To run the dotnetactor service some basic configurations are required to setup that are:

1. Before starting the service a service bus connection string needs to be setup as an environment variable as shown in the fig below. The process to setup the environment vaeriables is different in different operating systems. Follow this link and setup the Environment variable as shown in below figure according to your OS [Link](https://www.twilio.com/blog/2017/01/how-to-set-environment-variables.html)

![image](https://user-images.githubusercontent.com/28738233/119394639-b3c33180-bcd2-11eb-84f6-bc81698b0b7a.png)


# How to run the service

Open the terminal in the DotNetActorsHost project folder and enter the below command as shown below

dotnet run --SystemName=HelloCluster
-- RequestMsgTopic=actorsystem/actortopic
-- RequestMsgQueue=actorsystem/actorqueue
--ActorSystemName=actorsystem
--SubscriptionName=default

![image](https://user-images.githubusercontent.com/28738233/121670567-df556280-caad-11eb-9cb0-c4eba872842a.png)

Here 
SystemName creates the ActorSystem with this name.
RequestMsgTopic is the topic name form where the service subscribes its messages
RequestMsgQueue is the name of the queue on which response messages are pushed.
ActorSystemName is used to initialize the persistence provider

3. Thats it hit enter, the service is up and running
5. To run the service on multiple nodes, execute the same command on multiple terminals


