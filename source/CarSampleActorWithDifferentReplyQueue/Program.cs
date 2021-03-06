﻿using System;
using System.Threading;
using System.Threading.Tasks;
using ActorLibrary;
using DotNetActors.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CarSampleActorWithDifferentReplyQueue
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
            await LoadCarAttributes(logger);
        }
        
        private static async Task LoadCarAttributes(ILogger logger)
        {
            CancellationTokenSource src = new CancellationTokenSource();
            var cfg = GetLocaSysConfig();
            logger?.LogInformation("Car sample started with reply message queue as "+cfg.ReplyMsgQueue);
            logger?.LogInformation("Loaded Configuration, Messaging-Queue:"+cfg.ReplyMsgQueue+", Message-Topic:"+cfg.RequestMsgTopic);
            ActorSystem sysLocal = new ActorSystem($"CarFunctionalityTest", cfg);
            logger?.LogInformation("Created ActorSystem");
            
            logger?.LogInformation("Creating multiple Actor references");

            for (int i = 200; i < 300; i++)
            {
                ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(i);
                var response =  await actorRef1.Ask<CarAttributes>(new CarAttributes() {CarColor = "green", CarSpeed = "" + (222 + i), Persisted = false});
                logger?.LogInformation("Received result: "+response.Persisted);  
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
            cfg.ReplyMsgQueue = "actorsystem2/rcvlocal";
            cfg.RequestMsgTopic = "actorsystem/actortopic";
            cfg.ActorSystemName = "actorsystem2";
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