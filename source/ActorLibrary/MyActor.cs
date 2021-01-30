using AkkaSb.Net;
using System;

namespace ActorLibrary
{
    public class MyActor : ActorBase
    {
        public DeviceState State { get; set; }

        public MyActor(ActorId id) : base(id)
        {
            Receive<string>((str) =>
            {
               
                return null;
            });

            Receive<DeviceState>(((deviceState) =>
            {
                this.State = deviceState;
                return this.State;
            }));

            Receive<GetDeviceState>((devicestate) =>
            {
                return this.State;
            });

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

    public class GetDeviceState
    {
        public string Color { get; set; }

        public bool State { get; set; }
    }
}
