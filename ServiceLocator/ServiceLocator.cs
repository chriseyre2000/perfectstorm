using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Xml;

namespace PerfectStorm
{
    /// <summary>
    /// This is used to allow the creation of a class to be decoupled from the implementation.
    /// The client only needs to know the name and have a supported interface or base class.
    /// 
    /// The only restriction this imposes on the created class is that it must have an empty
    /// constructor.
    /// 
    /// The registry may be populated automatically from the appropriate config file or the 
    /// client may populate it itself.  By default the full name of the class is used, but 
    /// if client-populated then any unique string identifier will do.
    ///
    /// The beauty of this clarity of split is that the caller may not know any of the 
    /// implementation details. This ensures that the implementation can be replaced only
    /// requiring a configuration setting.
    /// 
    /// </summary>
    public static class ServiceLocator
    {
        static Dictionary<string, Type> _dict = new Dictionary<string, Type>();
        
        // This will be called before the first method on the type.
        static ServiceLocator()
        {
            XmlNode configNode = (XmlNode)ConfigurationManager.GetSection("PerfectStorm.ServiceLocator");
            if (configNode != null)
            {
                foreach (XmlNode node in configNode.SelectNodes("//PerfectStorm.ServiceLocator/assembly"))
                {
                    LoadAssembly(node.Attributes["name"].InnerText);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        public static void LoadAssembly(string Name)
        {
            Assembly a = Assembly.Load("Name");
            Type[] types = a.GetTypes();
            foreach (Type t in types)
            {
                if (!t.IsAbstract)
                {
                    // Only add if can be constructed.
                    if (t.GetConstructor(System.Type.EmptyTypes) != null)
                    {
                        AddType(t.FullName, t);
                    }
                }
            }
        
        }
        
        /// <summary>
        /// Creates an instance of a named class that conforms to the supplied interface or base type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns> 
        public static T CreateInstance<T>(string name)
        {
            Type t = null;
            if (_dict.ContainsKey(name))
            {
                t = _dict[name];
            }
            else
            { 
                throw new Exception(string.Format("ServiceLocator is Unable to create {0}", name));
            }
            
            return (T)Activator.CreateInstance(t);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks>Can only register classes with parameterless constructors.</remarks>
        public static void Register<T>() where T : new()
        {
            Type type = typeof(T);
            AddType(type.FullName, type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="T"></param>
        private static void AddType(string Name, Type T)
        {
            if (_dict.ContainsKey(Name))
            {
                _dict[Name] = T;
            }
            else
            {
                _dict.Add(Name, T);
            }        
        }

        /// <summary>
        /// Empties the registry.
        /// </summary>
        /// <remarks>This has been included to assist unit testing.</remarks>
        public static void Clear()
        {
            _dict.Clear();
        }
    }
}
