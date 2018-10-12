using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpcSlave
{
    class SlaveArgs
    {
        [Option('d', "hostName", Default = "localhost", Required = false, HelpText = "Specify host name")]
        public string HostName { get; set; }

        [Option("port", Default = 5001, Required = false, HelpText = "Specify the remote agent port")]
        public int Port { get; set; }

        [Option("dllFolder", Required = true, HelpText = "Specify the plugin's dll folder")]
        public string DllFolder { get; set; }

        [Option("moduleFullName", Required = true, HelpText = "Specify the full name of module to run")]
        public string ModuleFullName { get; set; }
    }
}
