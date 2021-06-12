# Hello World sample Implementation

Here step by step guidance is provided on how to run/imlement the client and dotnetactoservice 

# Client

In this section steps to implement and run the client are provided. An illustration of each step is also provided to ease the implementation.

## Step 2 : Implement your actor
Implement a class that extends the ActorBase class and implement the *Receive()* method as per the requirement as shown in the code snippet below.

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

The actor *MyActor* is a class that defines two methods by the message contract. We have aligned this service style to the *Akka* framework. The first operation will be invoked when the message of type *CarAttributes* is sent to the actor system. Analog, the second operation is invoked when the message of type *DeviceState* is sent to the service. In both cases, the operation can return some result or not. This is not defined by the contract. If you want to return a result return anything. This works because the contract expects an object.

## Step 2.
Implement a method that creates an ActorSystem object and invokes the operation that is executed remotely inside of the actor. The configuration should have the same topic, as used to start the service described later in this document. 

~~~csharp

private static async Task Run()
{
  ActorSbConfig cfg = new ActorSbConfig();
  cfg.SbConnStr = Environment.GetEnvironmentVariable("SbConnStr");
  cfg.ReplyMsgQueue = "actorsystem2/rcvlocal";
  cfg.RequestMsgTopic = "actorsystem/actortopic";
  cfg.ActorSystemName = "actorsystem2";
  
  ActorSystem sysLocal = new ActorSystem($"CarFunctionalityTest", cfg);
       
  for (int i = 0; i < 1000; i++)
  {
      ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(i);
      var response =  await actorRef1.Ask<CarAttributes>(new CarAttributes() {CarColor = "green", CarSpeed = "" + (222 + i), Persisted = false});
                
  }
}
            
~~~

# Service 

Please click [HERE](https://github.com/ddobric/dotnetactors/blob/branch-1/SystemConfigurations.md)  to run the dotnetactor service.





