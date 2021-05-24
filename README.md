# DotnetActors

# Abstract

To take the benefit of hardware concurrent computational models is growing rapidly. The advancements in cloud computing technologies such as the Microsoft Azure service bus allows improving the computational speed. This paper illustrates how the controllable, scalable and easy to use actor model system can be implemented. The API described in this paper relies on findings defined in the previous work that demonstrates how the computation of the model of the cortical column can be easily distributed by using Actor Programming Model approach. Actor model implementation provides a higher level of abstraction that makes it easier to write concurrent, parallel, and distributed systems. Actors are objects which encapsulate the state and behavior, they communicate exclusively by exchanging messages which are placed into the recipient's mailbox. The project described in this paper dotnetactors helps developers to easily leverage Actor Model programming paradigm and take a full control of the distribution of actors.

# Introduction

In this era of cloud computing and distribution, systems concurrency has played a vital role in achieving fast results with low latency. Concurrency is a property of the system to execute multiple activities at the same time. It means how components should work in a concurrent computational environment.

The actor model is a conceptual concurrent computation model, came into the picture in 1973. It establishes some of the rules on how the system’s components should behave and interact with each other. An actor in the actor model can be represented as a fundamental unit of computation and it can perform actions such as create another actor, send a message and designate how to handle the next message. Actors are lightweight and millions of them can be created very easily. Also, it is important to note that it takes fewer resources than threads. An actor has its private state and a mailbox, like a messaging queue. A message which an actor gets from another actor is stored in the mailbox and is processed in FIFO (first in and first out) order. Actors can be considered as the form of object-oriented programming which communicates by exchanging messages. Also, actors have a direct lifecycle that is they are not automatically destroyed when no longer referenced, and once created it is a user’s responsibility to eventually terminate them. This enables the user to control how the resources are released. Furthermore, in the case of distributed environments actors can communicate with each other through messages if they have the address of other actors. Actors can have local or remote addresses. The most widely used implementations for the Actor model are Akka and Erlang.

The main inspiration behind the actor model is to take full advantage of the hardware by using concurrency. Concurrency means that the ability of the system to perform different tasks simultaneously or out of order without affecting the outcome. DotNetActors is an essential element in the actor model and is responsible to handle the communication between the client and Microsoft’s Azure Service Bus. It was motivated by the research project in the field of artificial intelligence to scale the cortical algorithm in hierarchical temporal memory Spatial Pooler.





