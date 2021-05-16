using DotnetActorClientPair.Net;
using System;

namespace ActorLibrary
{
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

            Receive<string>((str) =>
            {
                return str + "Message from Actor";
            });

            Receive<DeviceState>(((deviceState) =>
            {
                deviceStateState = new DeviceState();
                deviceStateState.Color = "Blue";
                deviceStateState.State = false;
                return deviceStateState;
            }));

            Receive<long>((long num) =>
            {
                return CarSpeed;
            });

            Receive<DateTime>((DateTime dt) =>
            {
                
                return dt.AddDays(1);
            });
        }
    }

    /// <summary>
    /// Represents DeviceState class
    /// </summary>
    public class DeviceState
    {
        public string Color { get; set; }

        public bool State { get; set; }
    }
    
    /// <summary>
    /// Represents car Attributes
    /// </summary>
    public class CarAttributes
    {
        public String CarSpeed { get; set; }
        public String CarColor { get; set; }
        public Boolean Persisted { get; set; }

    }
}
