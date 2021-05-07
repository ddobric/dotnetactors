using System;
using System.Threading;
using System.Threading.Tasks;
using ActorLibrary;
using AkkaSb.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CarFunctionality
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args);
            builder.AddEnvironmentVariables();
            IConfigurationRoot configArgs = builder.Build();
            ILogger logger = generateLogger();

            if (configArgs["shallRun"] == "true")
            {
                await LoadCarAttributes(logger);
            }
            else
            {
                await CheckPersistence(logger);
            }
        }

        private static async Task LoadCarAttributes(ILogger logger)
        {
            logger?.LogInformation("Persist Car attributes started....");
            CancellationTokenSource src = new CancellationTokenSource();
            var cfg = GetLocaSysConfig();
            logger?.LogInformation("Loaded Configuration, Messaging-Queue:"+cfg.ReplyMsgQueue+", Message-Topic:"+cfg.RequestMsgTopic);
            ActorSystem sysLocal = new ActorSystem($"CarFunctionalityTest", cfg);
            logger?.LogInformation("Created ActorSystem");
            
            logger?.LogInformation("Creating multiple Actor references");

            for (int i = 1; i < 500; i++)
            {
                ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(i);
                var response = await actorRef1.Ask<CarAttributes>(new CarAttributes() {CarColor = "green", CarSpeed = "" + (222 + i), Persisted = false});
                logger?.LogInformation("Received result: "+response.Persisted);  
            }
        }
        
        private static async Task CheckPersistence(ILogger logger)
        {
            logger?.LogInformation("Fetching Car attributes started....");
            CancellationTokenSource src = new CancellationTokenSource();
            var cfg = GetLocaSysConfig();
            logger?.LogInformation("Loaded Configuration, Messaging-Queue:"+cfg.ReplyMsgQueue+", Message-Topic:"+cfg.RequestMsgTopic);
            ActorSystem sysLocal = new ActorSystem($"CarFunctionalityTest", cfg);
            logger?.LogInformation("Created ActorSystem");

            for (int i = 1; i < 500; i++)
            {
                ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(i);
                var response = await actorRef1.Ask<long>(i, routeToNode:"node1");
                logger?.LogInformation("Car Speed: "+response); 
            }
        }
        /// <summary>
        /// Gets the Local System configuration
        /// </summary>
        /// <returns></returns>
        private static ActorSbConfig GetLocaSysConfig()
        {
            ActorSbConfig cfg = new ActorSbConfig();
            cfg.SbConnStr = Environment.GetEnvironmentVariable("SbConnStr");
            cfg.ReplyMsgQueue = "actorsystem/rcvlocal";
            cfg.RequestMsgTopic = "actorsystem/actortopic";
            //cfg.TblStoragePersistenConnStr = tblAccountConnStr;
            cfg.ActorSystemName = "inst701";
            return cfg;
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