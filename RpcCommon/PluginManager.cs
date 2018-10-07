using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RpcCommon
{
    public class PluginManager<T>
    {
        private ICollection<Assembly> _assemblies;
        private ICollection<Type> _pluginTypes;

        public PluginManager(string dllFolder)
        {
            string[] dllFileNames = null;
            if (Directory.Exists(dllFolder))
            {
                dllFileNames = Directory.GetFiles(dllFolder, "*.dll");
            }
            _assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (string dllFile in dllFileNames)
            {
                var an = AssemblyName.GetAssemblyName(dllFile);
                //var assembly = Assembly.Load(an); // does not work, so use LoadFrom as a workaround
                var assembly = Assembly.LoadFrom(dllFile);
                _assemblies.Add(assembly);
            }
            var pluginType = typeof(T);
            _pluginTypes = new List<Type>();
            foreach (var assembly in _assemblies)
            {
                if (assembly != null)
                {
                    var types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                _pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }
        }

        public T Create(string fullName)
        {
            foreach (var type in _pluginTypes)
            {
                if (String.Equals(type.FullName, fullName, StringComparison.OrdinalIgnoreCase))
                {
                    var plugin = (T)Activator.CreateInstance(type);
                    return plugin;
                }
            }
            return default(T);
        }
    }
}
