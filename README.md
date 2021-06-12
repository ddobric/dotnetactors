[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://github.com/ddobric/htmdotnet/blob/master/LICENSE) ![.NET](https://github.com/ddobric/dotnetactors/workflows/.NET/badge.svg)

# DotnetActors

Actor Programming Model (APM) is an asynchronous message-passing model which is used for fine-grained concurrency and distributed-memory applications. The parallelism that is required for building concurrent applications can be easily provided by the Actor Programming Model (APM). 
The API described in this paper relies on the findings defined in the previous work that demonstrates how the computation of the model of the cortical column can be easily 
distributed by using the Actor Programming Model (APM) approach. 
The APM implementation provides a higher level of abstraction that makes it easier to write concurrent, parallel, and distributed systems. 
This simple framework helps .NET developers to easily leverage the Actor Programming Model (APM) paradigm and take full 
control of the distribution of the compute logic through actors. 
This project implements the lightweight Actor Programming Model framework as is extremely simple .NET/C# API. 
The **dotnetactors** lightweight framework was originally used to implement the parallel version of the Spatial Pooler algorithm published in this paper. After we tried to use several .NET Actor Model frameworks, we found that it is very difficult to take granular control of everything. So we implemented this simple framework that surprisingly 
Provides a full Actor Model with just a few lines of code. We also implemented a dedicated routing algorithm controlled on the client-side to be able to control at which node the actor will be executed. Typically, the client is not aware of the physical execution node of the invoked actor.
However, in some more complex scenarios like artificial intelligence, the client must be in control of the execution of the algorithm.
You can find more about such scenarios in this paper: https://www.researchgate.net/publication/343605270_Scaling_the_HTM_Spatial_Pooler.

The summary of this framework can also be found in the paper in the docs folder of this repository.

Have a fun.








