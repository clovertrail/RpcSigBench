using Common;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Common.RpcService;

namespace RpcSlave
{
    class RpcServiceImpl : RpcServiceBase
    {
        public override Task<Empty> TestConnection(Empty request, ServerCallContext context)
        {
            Console.WriteLine("Invoke TestConnection");
            return Task.FromResult(new Empty());
        }

        public override Task<CommandResponse> ExecuteCommand(CommandRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CommandResponse());
        }
    }
}
