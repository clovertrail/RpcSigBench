using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RpcCommon
{
    public class Factory
    {
        public static T Create<T>(string ns, string name) where T : class
        {
            var q = from t in Assembly.GetEntryAssembly().GetTypes()
                    where t.IsClass && t.Namespace == ns
                    select t;

            /*
            var q = from t in Assembly.GetEntryAssembly().GetTypes()
                    where t.IsClass &&
                    //string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase) &&
                    (typeof(T)).IsAssignableFrom(t)
                    select t;
                    */
            Type type = null;//  typeof(object);
            q.ToList().ForEach(t =>
            {
                if (string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase) && typeof(T).IsAssignableFrom(t))
                {
                    type = t;
                }
            });
            if (q.ToList().Count != 1 || type == null)
            {
                Console.WriteLine($"Cannot find the '{name}' {q.ToList().Count}");
                return default(T);
            }
            else if (type != null)
            {
                Console.WriteLine($"Find {name} {q.ToList().Count}");
            }
            T worker = (T)Activator.CreateInstance(type);
            return worker;
        }
    }
}
