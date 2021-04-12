using AkkaSb.Net;
using System;

namespace ActorLibrary
{
    /// <summary>
    /// Represents MyActor class
    /// </summary>
    public class MyActor : ActorBase
    {
        public DeviceState deviceStateState { get; set; }

        public MyActor(ActorId id) : base(id)
        {
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
                return num + 1;
            });

            Receive<DateTime>((DateTime dt) =>
            {
                
                return dt.AddDays(1);
            });
        }
    }

    public class DeviceState
    {
        public string Color { get; set; }

        public bool State { get; set; }
    }
}
