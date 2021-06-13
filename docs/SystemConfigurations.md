# System Configurations

Here are the basic configurations required to start the service.

## Step.1 : Configure the service bus connection string as an environment variable
The service bus connection string should be configured as an environment variable. The process to configure it is different in different operating systems. Please follow this [link](https://www.twilio.com/blog/2017/01/how-to-set-environment-variables.html) and setup the environment variable according to your OS.

After successfully configuring the connection string, it should appear in the environment variable as shown below:
![image](https://user-images.githubusercontent.com/28738233/119394639-b3c33180-bcd2-11eb-84f6-bc81698b0b7a.png)


## Step.2 : Start the service

1. To start the service, go inside DotNetActorsHost project folder and open the terminal or cmd there.
2. Now execute the below command in the termina or cmd.

~~~
dotnet run --SystemName=HelloCluster
-- RequestMsgTopic=actorsystem/actortopic
-- RequestMsgQueue=actorsystem/actorqueue
--ActorSystemName=actorsystem
--SubscriptionName=default
~~~

Here, 
- SystemName represents the system name that is used to initializes the ActorSystem. 
- RequestMsgTopic is defined as the topic name form where the service subscribes message. 
- RequestMsgQueue is described as  queue name to which response messages are pushed by the service .
- ActorSystemName is  interpretted as a name that is used to initialize the persistence provider.
For more information please refer [HERE](https://github.com/ddobric/dotnetactors/blob/main/docs/dotnetactors%20paper.pdf) for dotnet actor documentation

3. Once executed, the service will be up and running.
5. To run the service on multiple nodes, execute the same command over multiple terminals


