using ActorLibrary;
using AkkaSb.Net;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetactors
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CancellationTokenSource src = new CancellationTokenSource();

            var cfg = new ActorSbConfig();
            cfg.SbConnStr = Environment.GetEnvironmentVariable("SbConnStr");
            cfg.ReplyMsgQueue = "actorsystem/rcvlocal";
            cfg.RequestMsgTopic = "actorsystem/actortopic";
            cfg.TblStoragePersistenConnStr = null;
            cfg.ActorSystemName = "inst701";

            ActorSystem sysLocal = new ActorSystem($"Hello World", cfg);

            ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(1);

            var response = await actorRef1.Ask<long>((long)42);
                       
            response = actorRef1.Ask<long>((long)7).Result;

            ActorReference actorRef77 = sysLocal.CreateActor<MyActor>(77);

            var resp = await actorRef1.Ask<DeviceState>(new DeviceState() { Color = "green", State = true });

            var state = await actorRef1.Ask<DeviceState>(new GetDeviceState());


            ActorReference actorRef1017 = sysLocal.CreateActor<MyActor>(1017);

            resp = await actorRef1017.Ask<DeviceState>(new DeviceState() { Color = "blue", State = true });

            state = await actorRef1017.Ask<DeviceState>(new GetDeviceState());
        }
    }
}