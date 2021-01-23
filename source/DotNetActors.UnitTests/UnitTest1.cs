using AkkaSb.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ActorLibrary;
using System;

namespace DotNetActors.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [TestCategory("SbActorTests")]
        [TestCategory("RequiresActorHost")]
        public async Task AskClientTest()
        {
            Debug.WriteLine($"Start of {nameof(AskClientTest)}");

            CancellationTokenSource src = new CancellationTokenSource();

            var cfg = GetLocaSysConfig();

            ActorSystem sysLocal = new ActorSystem($"{nameof(AskClientTest)}/local", cfg);

            ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(1);

            var response = await actorRef1.Ask<long>((long)42);

            Assert.IsTrue(response == 43);

            response = actorRef1.Ask<long>((long)7).Result;

            Assert.IsTrue(response == 8);

            Debug.WriteLine($"End of {nameof(AskClientTest)}");
        }

        #region Private Members

        /// <summary>
        /// Please make sure that environment variable 'TblAccountConnStr' is set.
        /// </summary>
        public static string TblAccountConnStr
        {
            get
            {
                return Environment.GetEnvironmentVariable("TblAccountConnStr");
            }
        }

        /// <summary>
        /// Please make sure that environment variable 'SbConnStr' is set.
        /// </summary>
        public static string SbConnStr
        {
            get
            {
                return Environment.GetEnvironmentVariable("SbConnStr");
            }
        }

        /// <summary>
        /// When running unit tests set the correct system path: actorsystem2/rcvlocal, actorsystem2/actortopic
        /// </summary>
        /// <returns></returns>
        internal static ActorSbConfig GetLocaSysConfig()
        {
            ActorSbConfig cfg = new ActorSbConfig();
            cfg.SbConnStr = SbConnStr;
            cfg.ReplyMsgQueue = "actorsystem/rcvlocal";
            cfg.RequestMsgTopic = "actorsystem/actortopic";
            cfg.TblStoragePersistenConnStr = TblAccountConnStr;
            cfg.ActorSystemName = "inst701";
            return cfg;
        }

        internal static ActorSbConfig GetRemoteSysConfig(string node = "default")
        {
            var localCfg = GetLocaSysConfig();

            ActorSbConfig cfg = new ActorSbConfig();
            cfg.SbConnStr = SbConnStr;
            cfg.RequestMsgTopic = "actorsystem/actortopic";
            cfg.RequestSubscriptionName = node;
            cfg.ReplyMsgQueue = null;

            return cfg;
        }
        #endregion
    }
}
