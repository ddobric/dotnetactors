using System;
using System.Collections.Concurrent;
using AkkaSb.Net;

namespace CarActor
{
    public class CarBaseActor: ActorBase
    {
        public String CarSpeed { get; set; }
        public String CarColor { get; set; }
        public Boolean Persisted { get; set; }
        public CarBaseActor(ActorId id) : base(id)
        {
            Receive<CarAttributes>((CarAttributes carAttributes) =>
            {
                this.CarColor = carAttributes.CarColor;
                this.CarSpeed = carAttributes.CarSpeed;
                this.Persisted = true;
                this.Perist().Wait();
                return carAttributes;
            });
            
        }
    }
}