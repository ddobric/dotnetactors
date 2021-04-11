
using AkkaSb.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AkkaSb.Net
{
    public class ActorSbHostService
    {
        ILogger logger;

        private ActorSystem akkaClusterSystem;

        private CancellationTokenSource tokenSrc = new CancellationTokenSource();

        public ActorSbHostService(ILogger logger = null)
        {
            this.logger = logger;
        }

        public void Start(string[] args)
        {
            ActorSbConfig cfg = new ActorSbConfig();

            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args);
            builder.AddEnvironmentVariables();
            IConfigurationRoot configArgs = builder.Build();

            cfg.SbConnStr = configArgs["SbConnStr"];

            string rcvQueue = configArgs["ReplyMsgQueue"];
            if (!String.IsNullOrEmpty(rcvQueue))
                throw new ArgumentException("ReplyMsgQueue must not be specified when starting the server.");

            cfg.RequestMsgTopic = configArgs["RequestMsgTopic"];
            cfg.TblStoragePersistenConnStr = configArgs["TblStoragePersistenConnStr"];
            cfg.ActorSystemName = configArgs["ActorSystemName"];
            cfg.RequestSubscriptionName = configArgs["SubscriptionName"];
            string systemName = configArgs["SystemName"];

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                tokenSrc.Cancel();
            };

            BlobStoragePersistenceProvider prov = null;

            if (String.IsNullOrEmpty(cfg.TblStoragePersistenConnStr) == false)
            {
                prov = new BlobStoragePersistenceProvider();
                prov.InitializeAsync(cfg.ActorSystemName, new Dictionary<string, object>() { { "StorageConnectionString", cfg.TblStoragePersistenConnStr } }, purgeOnStart: false, logger: this.logger).Wait();
            }

            akkaClusterSystem = new ActorSystem($"{systemName}", cfg, logger, prov);
            akkaClusterSystem.Start(tokenSrc.Token);

            Console.WriteLine("Press any key to stop Actor SB system.");

        }

    }
}
