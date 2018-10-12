using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RpcCommon
{
    public class PluginManager
    {
        private ICollection<Assembly> _assemblies;

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
        }

        public object Create(string fullName)
        {
            foreach (var assembly in _assemblies)
            {
                var t = assembly.GetType(fullName);
                if (t != null)
                {
                    var obj = Activator.CreateInstance(t);
                    return obj;
                }
            }
            return null;
        }

        public object InvokeWithJsonParam(string fullName, string method, string jsonArg)
        {
            foreach (var assembly in _assemblies)
            {
                var t = assembly.GetType(fullName);
                if (t != null)
                {
                    var methodInfo = t.GetMethod(method);
                    if (methodInfo != null)
                    {
                        var parameters = methodInfo.GetParameters();
                        var o = Activator.CreateInstance(t);
                        if (parameters.Length == 1)
                        {
                            var argType = parameters[0].ParameterType;
                            // deserialize json arg to obj
                            var newtonSoftJsonConvert = typeof(Newtonsoft.Json.JsonConvert);
                            var genericMethod = newtonSoftJsonConvert.GetMethod("DeserializeObject", 1, new[] { typeof(string) }).MakeGenericMethod(new[] { argType });
                            var paraObj = genericMethod.Invoke(null, new object[] { jsonArg });
                            return methodInfo.Invoke(o, new object[] { paraObj });
                        }
                    }
                }
            }
            return null;
        }

        public object Invoke(string fullName, string method, object arg)
        {
            foreach (var assembly in _assemblies)
            {
                var t = assembly.GetType(fullName);
                if (t != null)
                {
                    var methodInfo = t.GetMethod(method);
                    if (methodInfo != null)
                    {
                        var parameters = methodInfo.GetParameters();
                        var o = Activator.CreateInstance(t);
                        if (parameters.Length == 1)
                        {
                            return methodInfo.Invoke(o, new object[] { arg });
                        }
                    }
                }
            }
            return null;
        }
    }

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
