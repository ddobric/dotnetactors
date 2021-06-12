[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://github.com/ddobric/htmdotnet/blob/master/LICENSE) ![.NET](https://github.com/ddobric/dotnetactors/workflows/.NET/badge.svg)

# DotnetActors

Actor Programming Model (APM) is an asynchronous message-passing model which is used for fine-grained concurrency and distributed-memory applications. The parallelism that is required for building concurrent applications can be easily provided by the Actor Programming Model (APM). The API described in this paper relies on the findings defined in the previous work that demonstrates how the computation of the model of the cortical column can be easily distributed by using the Actor Programming Model (APM) approach. The APM implementation provides a higher level of abstraction that makes it easier to write concurrent, parallel, and distributed systems. This paper helps developers to easily leverage the Actor Programming Model (APM) paradigm and take full control of the distribution of the compute logic through actors. 
This project implement the lightweith Actor Programmin Model framework as is extrimely simple .NET/C# API. The **dotnetactors** leitweight framework was used to impement the parallel version of the Spatial Pooler allgorithm published in this paper.
https://www.researchgate.net/publication/343605270_Scaling_the_HTM_Spatial_Pooler.

# Introduction

The limitations of the current processor technology have been a growing movement towards the use of processors with several cores [1]. The architectures of these processors aim in improving the throughput, efficiency, and processing power of the computer. To achieve this, one of the major hurdles is to build software that can leverage these facilities while still being a tractable programming model [1].  To overcome this Actor Programming Model (APM) has been implemented.

Actor Programming Model (APM) is a conceptual concurrent computation model which came into the picture in 1973 [2]. It has established some of the rules on how the system components should behave and interact with each other. An actor can be represented as a fundamental unit of computation that can perform certain actions such as create another actor, send a message to another actor, and designate how to handle the next message. Actors are lightweight and millions of them can be created very easily. It has its private state and a mailbox, like a messaging queue where a message from another actor can be stored. A message which an actor gets from another actor is processed in FIFO (first in and first out) order. Actors can be considered as the form of object-oriented programming, which communicates by exchanging messages. It has a direct lifecycle, which means they are not automatically destroyed when no longer referenced, once created it is the user’s responsibility to eventually terminate them. This enables the user to free up the resources depending upon the need. Actors generally communicate with each other through messages, if they have the address of other actors. The address can be local or remote. The most widely used implementations for the Actor Programming Model (APM) are Akka and Erlang [2].

For more information please refer to the paper in docs folder of this repository.







