using System;
using AkkaSb.Net;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ActorSbConfig cfg = new ActorSbConfig();

            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args);
            builder.AddEnvironmentVariables();
            IConfigurationRoot configArgs = builder.Build();
            Console.WriteLine("Hello World!");
            Console.WriteLine(configArgs["SbConnStr"].ToString());
        }
    }
}