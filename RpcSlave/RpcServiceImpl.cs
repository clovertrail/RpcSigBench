using Common;
using Grpc.Core;
using RpcCommon;
using RpcSDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Common.RpcService;

namespace RpcSlave
{
    class RpcServiceImpl : RpcServiceBase
    {
        private PluginManager _pluginManager;
        private string _moduleFullName;

        public RpcServiceImpl(PluginManager pluginManager, string moduleFullName)
        {
            _pluginManager = pluginManager;
            _moduleFullName = moduleFullName;
        }

        public override Task<Empty> TestConnection(Empty request, ServerCallContext context)
        {
            Console.WriteLine("Invoke TestConnection");
            return Task.FromResult(new Empty());
        }

        public override Task<CommandResponse> ExecuteCommand(CommandRequest request, ServerCallContext context)
        {
            Console.WriteLine($"cmd: {request.Command} {request.ArgumentsJsonContent}");
            _pluginManager.InvokeWithJsonParam(_moduleFullName, request.Command, request.ArgumentsJsonContent);
            return Task.FromResult(new CommandResponse());
        }

        private bool VerifyMasterSlaveModule(string slaveModuleFromMaster, string localSlaveModule)
        {
            if (!string.Equals(slaveModuleFromMaster, localSlaveModule, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Master and slave load different modules: {slaveModuleFromMaster} vs. {localSlaveModule}");
                return false;
            }
            return true;
        }
    }
}
