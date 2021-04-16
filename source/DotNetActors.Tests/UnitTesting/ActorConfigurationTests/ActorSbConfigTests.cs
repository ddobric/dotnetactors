using System.Collections.Generic;
using AkkaSb.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetActors.UnitTests.UnitTesting.ActorConfigurationTests
{
    /// <summary>
    /// Represents tests for the actor sb config class
    /// </summary>
    [TestClass]
    public class ActorSbConfigTests
    {
        [TestMethod]
        public void TestInitialisation()
        {
            ActorSbConfig actorSbConfig = new ActorSbConfig();
            Assert.IsNotNull(actorSbConfig);
            actorSbConfig.ActorSystemName = "ActorSystemName";
            actorSbConfig.TblStoragePersistenConnStr = "TableConnectionString";
            actorSbConfig.NumOfPartitions = 10;
            actorSbConfig.ReplyMsgQueue = "ReplyMessageQueue";
            actorSbConfig.RequestMsgTopic = "replyMessageTopic";
            actorSbConfig.RequestSubscriptionName = "SubscriptionName";
            
            Assert.AreEqual("ActorSystemName", actorSbConfig.ActorSystemName);
            Assert.AreEqual("TableConnectionString", actorSbConfig.TblStoragePersistenConnStr);
            Assert.AreEqual(10, actorSbConfig.NumOfPartitions);
            Assert.AreEqual("ReplyMessageQueue", actorSbConfig.ReplyMsgQueue);
            Assert.AreEqual("replyMessageTopic", actorSbConfig.RequestMsgTopic);
            Assert.AreEqual("SubscriptionName", actorSbConfig.RequestSubscriptionName);
            Assert.AreEqual(100, actorSbConfig.BatchSize);

        }
    }
}