using System;
using System.Collections.Generic;
using System.Text;

namespace RpcSDK
{
    public interface IAgent
    {

        /// <summary>
        /// Execute a command together with arguments on agent
        /// </summary>
        /// <param name="command"></param>
        /// <param name="argument"></param>
        void ExecuteCommand(string command, AbstractArguments arguments);
    }
}
