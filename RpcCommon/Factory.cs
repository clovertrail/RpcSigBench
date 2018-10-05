using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RpcCommon
{
    public class Factory
    {
        public static T Create<T>(string name) where T : class
        {
            var q = from t in Assembly.GetEntryAssembly().GetTypes()
                    where t.IsClass &&
                    string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase) &&
                    (typeof(T)).IsAssignableFrom(t)
                    select t;
            if (q.ToList().Count != 1)
            {
                Console.WriteLine($"Cannot find the '{name}' {q.ToList().Count}");
                return null;
            }
            else
            {
                Console.WriteLine($"Find {name} {q.ToList().Count}");
            }
            T worker = (T)Activator.CreateInstance(q.ToList()[0]);
            return worker;
        }
    }
}
