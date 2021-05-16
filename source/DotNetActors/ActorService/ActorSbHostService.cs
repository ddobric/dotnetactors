
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DotnetActorClientPair.Net
{
    /// <summary>
    /// Represents the Actor Service Bus Host Service
    /// </summary>
    public class ActorSbHostService
    {
        ILogger logger;

        private ActorSystem akkaClusterSystem;

        private CancellationTokenSource tokenSrc = new CancellationTokenSource();

        public ActorSbHostService(ILogger logger = null)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Represents the Start method
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <exception cref="ArgumentException">Exception if arguments are not passed correctly</exception>
        public void Start(string[] args)
        {
            ActorSbConfig cfg = new ActorSbConfig();

            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args);
            builder.AddEnvironmentVariables();
            IConfigurationRoot configArgs = builder.Build();
            cfg.SbConnStr = configArgs["SbConnStr"];
            
            // TODO: This is chetan's(FRAUAS student) storage connection string. We need to change it before commiting
            cfg.TblStoragePersistenConnStr =
                "DefaultEndpointsProtocol=https;AccountName=dotnetactors;AccountKey=mGLCq7CHPfy6Ivp23iU3hdDqGvEmyBxVAUkU1b89YeKVWKAHry3CTM7N7orGV0XCmhXdv0z7CgfYxh0MMj30Eg==;TableEndpoint=https://dotnetactors.table.cosmos.azure.com:443/;";
            
            //NOTE: providing right above
            // cfg.TblStoragePersistenConnStr = configArgs["TblStoragePersistenConnStr"];
            
            string rcvQueue = configArgs["ReplyMsgQueue"];
            if (!String.IsNullOrEmpty(rcvQueue))
                throw new ArgumentException("ReplyMsgQueue must not be specified when starting the server.");
            cfg.RequestMsgTopic = configArgs["RequestMsgTopic"];
            cfg.ActorSystemName = configArgs["ActorSystemName"];
            cfg.RequestSubscriptionName = configArgs["SubscriptionName"];
            string systemName = configArgs["SystemName"];

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                tokenSrc.Cancel();
            };

            TableStoragePersistenceProvider prov = null;

            if (String.IsNullOrEmpty(cfg.TblStoragePersistenConnStr) == false)
            {
                prov = new TableStoragePersistenceProvider();
                prov.InitializeAsync(cfg.ActorSystemName, new Dictionary<string, object>() { { "StorageConnectionString", cfg.TblStoragePersistenConnStr } }, purgeOnStart: false, logger: this.logger).Wait();
            }

            akkaClusterSystem = new ActorSystem($"{systemName}", cfg, logger, prov);
            akkaClusterSystem.Start(tokenSrc.Token);
            Console.WriteLine("Press any key to stop Actor SB system.");

        }

    }
}
