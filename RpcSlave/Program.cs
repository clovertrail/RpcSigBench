using CommandLine;
using Common;
using Grpc.Core;
using RpcSlave;
using System;

namespace RpcMaster
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MachineName: {0}", Environment.MachineName);
            var slaveArgs = ParseArgs(args);
            var server = new Server(new ChannelOption[]
            {
                // For Group, the received message size is very large, so here set 8000k
                new ChannelOption(ChannelOptions.MaxReceiveMessageLength, 8192000)
            })
            {
                Services = { RpcService.BindService(new RpcServiceImpl()) },
                Ports = { new ServerPort(slaveArgs.HostName, slaveArgs.Port, ServerCredentials.Insecure) }
            };
            server.Start();
            Console.WriteLine($"Server [{slaveArgs.HostName}:{slaveArgs.Port}] started");

            server.ShutdownAsync().Wait();
        }

        private static SlaveArgs ParseArgs(string[] args)
        {
            bool invalidOptions = false;
            var argsOption = new SlaveArgs();
            var result = Parser.Default.ParseArguments<SlaveArgs>(args)
                .WithParsed(options => argsOption = options)
                .WithNotParsed(error => {
                    Console.WriteLine($"Fail to parse the options: {error}");
                    invalidOptions = true;
                });
            if (invalidOptions)
                return null;
            return argsOption;
        }
    }
}
