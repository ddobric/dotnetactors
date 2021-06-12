using ActorLibrary;
using DotNetActors.Net;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotNetActorsSample
{
    class Program
    {
        /// <summary>
        /// Represents Main class that initialise the client and asks a response from the service
        /// </summary>
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            CancellationTokenSource src = new CancellationTokenSource();
            ILogger logger = generateLogger();
            var cfg = new ActorSbConfig();
            cfg.SbConnStr = Environment.GetEnvironmentVariable("SbConnStr");
            cfg.ReplyMsgQueue = "actorsystem/rcvlocal";
            cfg.RequestMsgTopic = "actorsystem/actortopic";
            cfg.TblStoragePersistenConnStr = null;
            cfg.ActorSystemName = "inst701";
            logger?.LogInformation("Loaded Configuration, Messaging-Queue:"+cfg.ReplyMsgQueue+", Message-Topic:"+cfg.RequestMsgTopic);
            ActorSystem sysLocal = new ActorSystem($"TestDotNetWorking", cfg);
            logger?.LogInformation("Created ActorSystem");
            ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(1);
            ActorReference actorRef2 = sysLocal.CreateActor<MyActor>(77);
            logger?.LogInformation("Created two Actor references, actorReference1: "+actorRef1+", actorReference2: "+actorRef2);
            logger?.LogInformation("Asking 42 long value from actorReference1");
            var response = await actorRef1.Ask<long>((long)42);
            logger?.LogInformation("Received result: "+response);
            logger?.LogInformation("Asking 7 long value from actorReference1");
            response = actorRef1.Ask<long>((long)7).Result;
            logger?.LogInformation("Received result: "+response);
            var resp = await actorRef1.Ask<DeviceState>(new DeviceState() { Color = "green", State = true });
            var stringResponse = await actorRef1.Ask<DateTime>(DateTime.Now);
            logger?.LogInformation("Successfully tested the functionality");
        }

        /// <summary>
        /// Method to enable logging
        /// </summary>
        /// <returns></returns>
        private static ILogger generateLogger()
        {
            ILoggerFactory factory = LoggerFactory.Create(logBuilder =>
            {
                logBuilder.AddDebug();
                logBuilder.AddConsole();
            });

            return factory.CreateLogger(nameof(Program));
        }
    }
}