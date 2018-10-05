using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpcSlave
{
    class SlaveArgs
    {
        [Option('d', "hostName", Default = "0.0.0.0", Required = false, HelpText = "Specify host name")]
        public string HostName { get; set; }

        [Option("port", Default = 7000, Required = false, HelpText = "Specify the remote agent port")]
        public int Port { get; set; }
    }
}
