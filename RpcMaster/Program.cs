using CommandLine;
using Common;
using Grpc.Core;
using RpcCommon;
using RpcSDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RpcMaster
{
    class Program
    {
        static void Main(string[] args)
        {
            var argsOption = ParseArgs(args);
            if (argsOption == null)
                return;
            var master = new Master(argsOption);
            master.WaitAgentsReady();
            master.Load();
            master.InitAgents();
            master.RunAgents();
            master.WaitShutDown();
        }

        private static Args ParseArgs(string[] args)
        {
            bool invalidOptions = false;
            var argsOption = new Args();
            var result = Parser.Default.ParseArguments<Args>(args)
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
