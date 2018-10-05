﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpcMaster
{
    public class Args
    {
        [Option("agentList", Required = true, HelpText = "Specify agent (hostname or IP) list, separated by ';'")]
        public string AgentList { get; set; }

        [Option("port", Default = 7000, Required = false, HelpText = "Specify the remote agent port")]
        public int Port { get; set; }

        [Option("moduleName", Required = true, HelpText = "Specify full name of module to run")]
        public string ModuleName { get; set; }

        [Option("moduleConfigFile", Required = true, HelpText = "Specify the configuration file for the module")]
        public string ModuleConfigFile { get; set; }
    }
}
