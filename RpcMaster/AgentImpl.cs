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

        public AgentImpl(RpcServiceClient client)
        {
            _client = client;
        }

        public void ExecuteCommand(string command, AbstractArguments arguments)
        {
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(arguments);
            var commandRequest = new CommandRequest()
            {
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
