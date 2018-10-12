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
    internal class Master
    {
        private List<Channel> _channels;
        private List<RpcService.RpcServiceClient> _clients;
        private List<IAgent> _connectedAgents;
        private IModule _module;
        private Args _args;
        private AbstractOptions _moduleConfigOptions;
        private PluginManager<IModule> _pluginManager;

        public Master(Args args)
        {
            //Console.WriteLine($"{GetType().Name}");
            _args = args;
            var agents = new List<string>(args.AgentList.Split(';'));
            _channels = CreateChannels(agents, args.Port);
            _clients = CreateRpcConnections(_channels);
        }

        public bool Load()
        {
            _module = LoadModule();
            if (_module == null)
                return false;
            _connectedAgents = CreateConnectedAgents(_clients, _module.GetType().FullName);
            string content = null;
            using (StreamReader sr = new StreamReader(_args.ModuleConfigFile))
            {
                content = sr.ReadToEnd();
            }
            _moduleConfigOptions = _module.Parse(content);
            return true;
        }

        public void InitAgents()
        {
            _module.Init(_connectedAgents, _moduleConfigOptions);
        }

        public void RunAgents()
        {
            _module.Run(_connectedAgents, _moduleConfigOptions);
        }

        public void WaitShutDown()
        {
            for (var i = 0; i < _channels.Count; i++)
            {
                _channels[i].ShutdownAsync().Wait();
            }
        }

        private IModule LoadModule()
        {
            _pluginManager = new PluginManager<IModule>(_args.DllFolder);
            var moduleFullName = _args.ModuleFullName;
            var module = _pluginManager.Create(moduleFullName);
            return module;
        }

        private List<Channel> CreateChannels(List<string> agents, int rpcPort)
        {
            var channels = new List<Channel>(agents.Count);
            for (var i = 0; i < agents.Count; i++)
            {
                Console.WriteLine($"add {agents[i]}:{rpcPort}");
                channels.Add(new Channel($"{agents[i]}:{rpcPort}", ChannelCredentials.Insecure,
                    new ChannelOption[] {
                        // For Group, the received message size is very large, so here set 8000k
                        new ChannelOption(ChannelOptions.MaxReceiveMessageLength, 8192000)
                    }));
            }
            return channels;
        }

        private static List<IAgent> CreateConnectedAgents(List<RpcService.RpcServiceClient> agentConnections, string moduleFullName)
        {
            var connectedAgents = new List<IAgent>(agentConnections.Count);
            foreach (var agentConnection in agentConnections)
            {
                connectedAgents.Add(new AgentImpl(agentConnection, moduleFullName));
            }
            return connectedAgents;
        }

        private static List<RpcService.RpcServiceClient> CreateRpcConnections(List<Channel> channels)
        {
            var clients = new List<RpcService.RpcServiceClient>();
            for (var i = 0; i < channels.Count; i++)
            {
                clients.Add(new RpcService.RpcServiceClient(channels[i]));
            }
            return clients;
        }

        public void WaitAgentsReady()
        {
            var agents = _clients;
            var empty = new Empty();
            while (true)
            {
                try
                {
                    foreach (var client in agents)
                    {
                        try
                        {
                            client.TestConnection(empty);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Fail to connect for {e.Message}");
                            throw;
                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }
                break;
            }
        }
    }
}
