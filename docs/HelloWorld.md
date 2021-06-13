# Hello World sample Implementation

Here step by step guidance is provided on how to run/imlement the client and dotnetactoservice 

# Client

In this section steps to implement and run the client are provided. An illustration of each step is also provided to ease the implementation.

## Step 1 : Implement the actor
The actor is a class that implements the code to be exectuded remotelly. To create an actor, implement a cnew lass that derives from *ActorBase* clas. The compute logic is implemented in various *Receive()* methods.

~~~csharp
 /// <summary>
    /// Represents MyActor class
    /// </summary>
    public class MyActor : ActorBase
    {
        public DeviceState deviceStateState { get; set; }
        public String CarSpeed { get; set; }
        public String CarColor { get; set; }
        public Boolean Persisted { get; set; }

        public MyActor(ActorId id) : base(id)
        {
            Receive<CarAttributes>((CarAttributes carAttributes) =>
            {
                this.CarColor = carAttributes.CarColor;
                this.CarSpeed = carAttributes.CarSpeed;
                this.Persisted = true;
                this.Perist().Wait();
                carAttributes.Persisted = true;
                return carAttributes;
            });
         
            Receive<DeviceState>(((deviceState) =>
            {
                deviceStateState = new DeviceState();
                deviceStateState.Color = "Blue";
                deviceStateState.State = false;
                return deviceStateState;
            }));          
        }
    }
~~~

The actor *MyActor* is a class that defines two operations defined by the message contract. We have aligned this service style to the *Akka* framework. The first operation will be invoked when the message of type *CarAttributes* is sent to the actor system. Analog, the second operation is invoked when the message of type *DeviceState* is sent to the service. In both cases, the operation can return some result or not. This is not defined by the contract. If you want to return a result return anything. This works because the contract expects an object. As a object oriented developer, you migth find this a bit strange, but the Actor Model is not the Object Oriented programming. We use here OO language C# to make it as intuitive as possible.

## Step 2.
Implement a method that creates an *ActorSystem* object and invokes the operation that is executed remotely inside of the actor. The *ActorSystem* is your **dotnetactors-API**. Pleas note that the configuration should have the same topic, as used to start the service described later in this document. 

~~~csharp

private static async Task Run()
{
  ActorSbConfig cfg = new ActorSbConfig();
  cfg.SbConnStr = Environment.GetEnvironmentVariable("SbConnStr");
  cfg.ReplyMsgQueue = "actorsystem2/rcvlocal";
  cfg.RequestMsgTopic = "actorsystem/actortopic";
  cfg.ActorSystemName = "actorsystem2";
  
  ActorSystem sysLocal = new ActorSystem($"NAME OF YOUR CHOICE", cfg);
       
  for (int i = 0; i < 1000; i++)
  {
      // Create the reference (proxy) to the actor.
      ActorReference actorRef = sysLocal.CreateActor<MyActor>(i);

      // Invoke the operation defined by CarAttributes type and 
      // wait on result. If the operation does not return anything 
      // (void) the use actorRef.Tell method.
      var response =  await actorRef.Ask<CarAttributes>(
      new CarAttributes() 
      {
          CarColor = "green", 
          CarSpeed = "" + (222 + i), 
          Persisted = false});                
     }
}
            
~~~

This is really all code you need.

# Service 

To run the actor on the service side, you have to implement the console application (or any other applicaiton type if you like) that will host the actor. Hosting of the actor means, you need a process that will run the actor operation when it is invoked bu *Ask* and *Tell* methods. These methods send a message that describe which actor and which operation needs to be invoked.

Create the console application, reference **dotnetactors** nuget package, paste following code and reference the library with implementation of your actor(s).

~~~csharp
        static void Main(string[] args)
        {
            ILoggerFactory factory = LoggerFactory.Create(logBuilder =>
            {
                logBuilder.AddDebug();
                logBuilder.AddConsole();
            });
                        
            ActorSbHostService svc = 
            new ActorSbHostService(
            factory.CreateLogger(nameof(ActorSbHostService)));
            svc.Start(args);
        }
~~~

There is nothing more you have to do.

However, to run this code, you will have to provide few command line arguments. The *ActorSbServiceHost* will grab arguments from the command line arguments and from environment variables. Environment variables (if exist the same one) will override command line arguments.


For more information about configuration please refer [HERE](https://github.com/ddobric/dotnetactors/blob/main/docs/SystemConfigurations.md)  to run the dotnetactor service.





