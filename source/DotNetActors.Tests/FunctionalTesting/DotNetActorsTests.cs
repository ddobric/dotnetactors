// Copyright (c) Damir Dobric. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using DotNetActors.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace DotNetActorTests
{
    [TestClass]
    public class DotNetActorsTests
    {
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
        /// Gets the Local System configuration
        /// </summary>
        /// <returns></returns>
        internal static ActorSbConfig GetLocaSysConfig()
        {
            ActorSbConfig cfg = new ActorSbConfig();
            cfg.SbConnStr = SbConnStr;
            cfg.ReplyMsgQueue = "actorsystem/rcvlocal";
            cfg.RequestMsgTopic = "actorsystem/actortopic";
            //cfg.TblStoragePersistenConnStr = tblAccountConnStr;
            cfg.ActorSystemName = "inst701";
            return cfg;
        }

        /// <summary>
        /// Gets the remote system configurations
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
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


        static ConcurrentDictionary<object, object> receivedMessages = new ConcurrentDictionary<object, object>();


        /// <summary>
        /// Represents the Test class
        /// </summary>
        public class TestClass
        {
            public int Prop1 { get; set; }

            public string Prop2 { get; set; }
        }
        
        /// <summary>
        /// Represents the Device state
        /// </summary>
        public class DeviceState
        {
            public string Color { get; set; }

            public bool State { get; set; }
        }

        /// <summary>
        /// Represents MyActor Class
        /// </summary>
        public class MyActor : ActorBase
        {
            public MyActor(ActorId id) : base(id)
            {
                Receive<string>((str) =>
                {
                    receivedMessages.TryAdd(str, str);
                    return null;
                });
                
                Receive<DeviceState>((DeviceState) =>
                {
                    receivedMessages.TryAdd(DeviceState, DeviceState);
                    DeviceState.Color = "braun";
                    return DeviceState;
                });

                Receive<TestClass>(((c) =>
                {
                    receivedMessages.TryAdd(c, c.ToString());
                    return null;
                }));

                Receive<long>((long num) =>
                {
                    receivedMessages.TryAdd(num, num);
                    return num + 1;
                });

                Receive<DateTime>((DateTime dt) =>
                {
                    receivedMessages.TryAdd(dt, dt);
                    return dt.AddDays(1);
                });
            }
        }


        /// <summary>
        /// Tests if Tell() works as designed.
        /// </summary>

        [TestMethod]
        [TestCategory("SbActorTests")]
        public void TellTest()
        {
            Debug.WriteLine($"Start of {nameof(TellTest)}");
            var cfg = GetLocaSysConfig();
            ActorSystem sysLocal = new ActorSystem($"{nameof(TellTest)}/local", cfg);
            ActorSystem sysRemote = new ActorSystem($"{nameof(TellTest)}/remote", GetRemoteSysConfig());
            CancellationTokenSource src = new CancellationTokenSource();

            var task = Task.Run(() =>
            {
                sysRemote.Start(src.Token);
            });

            ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(1);
            actorRef1.Tell("message 1").Wait();
            ActorReference actorRef2 = sysLocal.CreateActor<MyActor>(2);
            actorRef2.Tell("message 2").Wait();

            while (true)
            {
                if (receivedMessages.Count == 2)
                {
                    Assert.IsTrue(receivedMessages.Values.Contains("message 1"));
                    Assert.IsTrue(receivedMessages.Values.Contains("message 1"));
                    src.Cancel();
                    break;
                }
                Thread.Sleep(1000);
            }

            task.Wait();

            Debug.WriteLine($"End of {nameof(TellTest)}");
        }

        /// <summary>
        /// Tests if Ask() works as designed.
        /// </summary>

        [TestMethod]
        [TestCategory("SbActorTests")]
        public void AskTest()
        {
            Debug.WriteLine($"Start of {nameof(AskTest)}");

            ActorSystem sysRemote = new ActorSystem($"{nameof(AskTest)}/remote", GetRemoteSysConfig());

            CancellationTokenSource src = new CancellationTokenSource();

            var task = Task.Run(() =>
            {
                sysRemote.Start(src.Token);
            });

            var cfg = GetLocaSysConfig();

            ActorSystem sysLocal = new ActorSystem($"{nameof(AskTest)}/local", cfg);

            ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(1);

            var response = actorRef1.Ask<long>((long)42).Result;

            Assert.IsTrue(response == 43);

            response = actorRef1.Ask<long>((long)7).Result;

            Assert.IsTrue(response == 8);

            Debug.WriteLine($"End of {nameof(AskTest)}");
        }

      
        /// <summary>
        /// Tests if Ask() works as designed.
        /// </summary>

        [TestMethod]
        [TestCategory("SbActorTests")]
        public void AskManyNodesTest()
        {
            var cfg = GetLocaSysConfig();
            ActorSystem sysLocal = new ActorSystem("local", cfg);
            ActorSystem sysRemote1 = new ActorSystem("node1", GetRemoteSysConfig());
            ActorSystem sysRemote2 = new ActorSystem("node2", GetRemoteSysConfig());

            CancellationTokenSource src = new CancellationTokenSource();

            var task1 = Task.Run(() =>
            {
                sysRemote1.Start(src.Token);
            });

            var task2 = Task.Run(() =>
            {
                sysRemote2.Start(src.Token);
            });

            ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(1);
            var response = actorRef1.Ask<long>((long)42).Result;
            Assert.IsTrue(response == 43);

            response = actorRef1.Ask<long>((long)7).Result;
            Assert.IsTrue(response == 8);

            ActorReference actorRef2 = sysLocal.CreateActor<MyActor>(7);
            var response2 = actorRef2.Ask<long>((long)10).Result;
            Assert.IsTrue(response2 == 11);

            DateTime dtRes = actorRef2.Ask<DateTime>(new DateTime(2019, 1, 1)).Result;

            Assert.IsTrue(dtRes.Day == 2);
            Assert.IsTrue(dtRes.Year == 2019);
            Assert.IsTrue(dtRes.Month == 1);
        }


        /// <summary>
        /// Tests if Ask() works as designed.
        /// </summary>

        [TestMethod]
        [TestCategory("SbActorTests")]
        public void AskManyNodesManyMessagesTest()
        {
            var cfg = GetLocaSysConfig();
            ActorSystem sysLocal = new ActorSystem("local", cfg);
            ActorSystem sysRemote1 = new ActorSystem("remote1", GetRemoteSysConfig());
            ActorSystem sysRemote2 = new ActorSystem("remote2", GetRemoteSysConfig());

            CancellationTokenSource src = new CancellationTokenSource();

            var task1 = Task.Run(() =>
            {
                sysRemote1.Start(src.Token);
            });

            var task2 = Task.Run(() =>
            {
                sysRemote2.Start(src.Token);
            });

            //Thread.Sleep(int.MaxValue);

            Parallel.For(0, 20, (i) =>
            {
                ActorReference actorRef = sysLocal.CreateActor<MyActor>(i);

                for (int k = 0; k < 5; k++)
                {
                    var response = actorRef.Ask<long>((long)k).Result;
                    Assert.IsTrue(response == k + 1);

                    DateTime dtRes = actorRef.Ask<DateTime>(new DateTime(2019, 1, 1 + i % 17)).Result;

                    Assert.IsTrue(dtRes.Day == 2 + i % 17);
                    Assert.IsTrue(dtRes.Year == 2019);
                    Assert.IsTrue(dtRes.Month == 1);
                }
            });
        }
        
        [TestMethod]
        [TestCategory("SbActorTests")]
        public void AskTestDeviceState()
        {
            Debug.WriteLine($"Start of {nameof(AskTestDeviceState)}");

            ActorSystem sysRemote = new ActorSystem($"{nameof(AskTest)}/remote", GetRemoteSysConfig());

            CancellationTokenSource src = new CancellationTokenSource();

            var task = Task.Run(() =>
            {
                sysRemote.Start(src.Token);
            });

            var cfg = GetLocaSysConfig();

            ActorSystem sysLocal = new ActorSystem($"{nameof(AskTest)}/local", cfg);

            ActorReference actorRef1 = sysLocal.CreateActor<MyActor>(1);

            var response = actorRef1.Ask<DeviceState>(new DeviceState() {Color = "green", State = true}).Result;
            Assert.AreEqual(response.Color, "braun");
            Assert.IsTrue(response.State);
            Debug.WriteLine($"End of {nameof(AskTestDeviceState)}");
        }
    }
}
