using AkkaSb.Net;
using System;

namespace ActorLibrary
{
    public class MyActor : ActorBase
    {
        public MyActor(ActorId id) : base(id)
        {
            Receive<string>((str) =>
            {
               
                return null;
            });

            Receive<TestClass>(((c) =>
            {
                
                return null;
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

    public class TestClass
    {
        public int Prop1 { get; set; }

        public string Prop2 { get; set; }
    }
}
