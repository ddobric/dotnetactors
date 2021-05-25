# Hello World sample Implementation

Here step by step guidance is provided on how to run/imlement the client and dotnetactoservice 

# Client

In this section steps to implement and run the client are provided. An illustration of each step is also provided to ease the implementation.

Step 1. The first step is to implement a method and create an ActorSystem object. Then pass the configurations as shown in Fig below. The configuration should have the same topic, as used to start the service. 

![image](https://user-images.githubusercontent.com/28738233/119395433-c8ec9000-bcd3-11eb-913b-c2d8f268f52a.png)


Step 2. Implement a class that extends the ActorBase class and implement the Receive() method as per the requirement as shown

![image](https://user-images.githubusercontent.com/28738233/119395475-da359c80-bcd3-11eb-9df3-034d6547896e.png)

Step 3. Using the ActorSystem object, create an actor as shown in Fig below. Then call Ask() method to get the result from the service. That’s it, run this method from the Main() method.

![image](https://user-images.githubusercontent.com/28738233/119395509-e588c800-bcd3-11eb-80b4-f2534f01918d.png)


# Service 

Please click [HERE](https://github.com/ddobric/dotnetactors/blob/branch-1/SystemConfigurations.md)  to run the dotnetactor service.





