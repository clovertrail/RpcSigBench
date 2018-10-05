using System;
using System.Collections.Generic;
using System.Text;

namespace RpcSDK
{
    public interface IModule
    {
        /// <summary>
        /// Verify the input content and marshal it to an <see cref="AbstractOptions"/> object
        /// </summary>
        /// <param name="optionContent">string content with Json format</param>
        /// <returns>an <see cref="AbstractOptions"/> object if the input is valid, otherwise null</returns>
        AbstractOptions Parse(string optionContent);

        /// <summary>
        /// Initialize all agents through options
        /// </summary>
        /// <param name="agents">agents needed to be initialized</param>
        /// <param name="options">options to initialize the agents</param>
        void Init(List<IAgent> agents, AbstractOptions options);

        /// <summary>
        /// Run all agents with the options
        /// </summary>
        /// <param name="agents">agents needed to run</param>
        /// <param name="options">options to run</param>
        void Run(List<IAgent> agents, AbstractOptions options);
    }
}
