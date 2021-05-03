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
            
            ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(1);
            logger?.LogInformation("Created Actor reference, actorReference1: "+actorRef1);
            logger?.LogInformation("Asking from actorReference1");
            var response = await actorRef1.Ask<CarAttributes>(new CarAttributes() {CarColor = "green", CarSpeed = "222km/hr", Persisted = false});
            logger?.LogInformation("Received result: "+response.Persisted);  
        }
        
        private static async Task CheckPersistence(ILogger logger)
        {
            logger?.LogInformation("Fetching Car attributes started....");
            CancellationTokenSource src = new CancellationTokenSource();
            var cfg = GetLocaSysConfig();
            logger?.LogInformation("Loaded Configuration, Messaging-Queue:"+cfg.ReplyMsgQueue+", Message-Topic:"+cfg.RequestMsgTopic);
            ActorSystem sysLocal = new ActorSystem($"CarFunctionalityTest", cfg);
            logger?.LogInformation("Created ActorSystem");
            
            ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(200);
            logger?.LogInformation("Created Actor reference, actorReference1: "+actorRef1);
            logger?.LogInformation("Asking from actorReference1");
            var response = await actorRef1.Ask<CarAttributes>(new CarAttributes() {CarColor = "green", CarSpeed = "222km/hr", Persisted = true});
            logger?.LogInformation("Received result: "+response.CarSpeed); 
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