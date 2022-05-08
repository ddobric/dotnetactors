[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://github.com/ddobric/htmdotnet/blob/master/LICENSE) ![.NET](https://github.com/ddobric/dotnetactors/workflows/.NET/badge.svg)

# Dotnet Actors

Actor Programming Model (APM) is an asynchronous message-passing model which is used for fine-grained concurrency and distributed-memory applications. The parallelism that is required for building concurrent applications can be easily provided by the Actor Programming Model (APM). 

The API implemeted in this repository relies on the findings defined in the research that demonstrates how the computation of the model of the cortical mini-column can be easily distributed by using the Actor Programming Model (APM) approach. 

The APM implementation provides a higher level of abstraction that makes it easier to write concurrent, parallel, and distributed systems. 
This lightweight framework helps .NET developers to easily leverage the Actor Programming Model (APM) paradigm and take full 
control of the distribution of the compute logic through actors. Typically, in an APM the client is not aware of the physical execution node of the invoked actor (location transparency).This framework does not use the location transparency as defned by original APM paper. It rather takes a full controll of the partitionng at the client side to be able to better distribute the computation. Thi smakes more complicate to implement typical enterprise applicaitons, but it allows a better controll of distribution of the compute logic. 

This project is the implemntation of the lightweight Actor Programming Model framework with very intuitive and simple .NET/C# API. 
The **dotnetactors** lightweight framework was originally used to implement the parallel version of the Spatial Pooler algorithm published in paper bellow. After several .NET Actor Models have been tested, it was found that it is very difficult to take granular control of everything in each tested framework. 
This very simple framework surprisingly provides a full Actor Model with just a few lines of code. It also implements a dedicated routing algorithm controlled on the client-side to be able to control at which node the actor will be executed.  

You can find more about this in this paper: https://www.researchgate.net/publication/343605270_Scaling_the_HTM_Spatial_Pooler.

The summary of this framework can also be found in the paper in the docs folder of this repository.

Have a fun.








