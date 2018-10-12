using Common;
using RpcSDK;
using System;
using System.Collections.Generic;
using System.Text;
using static Common.RpcService;

namespace RpcMaster
{
    public class AgentImpl : IAgent
    {
        private RpcServiceClient _client;
        private string _moduleFullName;

        public AgentImpl(RpcServiceClient client, string moduleFullName)
        {
            _client = client;
            _moduleFullName = moduleFullName;
        }

        public void ExecuteCommand(string command, AbstractArguments arguments)
        {
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(arguments);
            var commandRequest = new CommandRequest()
            {
                FullModuleName = _moduleFullName,
                Command = command,
                ArgumentsJsonContent = jsonStr
            };
            var result = _client.ExecuteCommand(commandRequest);
            if (result.HasError)
            {
                // handle error
            }
            else
            {

            }
        }
    }
}
