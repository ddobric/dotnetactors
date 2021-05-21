using System;
using System.Threading;
using System.Threading.Tasks;
using ActorLibrary;
using DotNetActors.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var sbMsg = CreateMessage(msg, false, actorType, actorId, routeToNode);
            await this.RequestMsgSenderClient.SendAsync(sbMsg);
        }
    }
}