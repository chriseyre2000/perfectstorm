using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NDesk.Options;

using PerfectStorm.CodeGenLibrary;

namespace PerfectStorm.CodeGen
{
    /// <summary>
    /// This is now fully decoupled.
    /// </summary>
    class Program
    {
        static TransformContext _context = null;
        static IConfiguration _config = null;
        static ILogger _logger = null;
        static ITransformOutput _transformOutput = null;
        static List<ITransformFilter> _filters = null;

        /// <summary>
        /// This is the dynamic loading part of an IoC framework
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        static object CreateInstance(string assembly, string className)
        {
            Assembly localAssembly = Assembly.Load(assembly);
            Type type = localAssembly.GetType(className);
            object newInstance = Activator.CreateInstance(type);
            return newInstance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="config"></param>
        /// <remarks>If this only passes in the global config object then a given
        /// filter may not have multiple parameter sets.
        /// 
        /// So pass the config object of the sub node of the item declaring the filter.
        /// </remarks>
        static void InitPlugin(object plugin, IConfiguration config)
        {
            if (plugin as IPluginInit != null)
            {
                (plugin as IPluginInit).Init(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="className"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        static object CreateAndInit(string assembly, string className, IConfiguration config)
        {
            object result = null;
            result = CreateInstance(assembly, className);
            InitPlugin(result, config);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string configFile = null;

            var p = new OptionSet() {
   	        { "config=",      v => configFile = v },
            };
            List<string> extraArgs = p.Parse(args);

            if (configFile == null)
            {
                _config = new AppConfigConfiguration();
            }
            else
            {
                _config = new FileAppConfigConfiguration(configFile);
            }

            _logger = CreateAndInit(_config.GetConfigData("plugins", "logger/@assembly"),
                                    _config.GetConfigData("plugins", "logger/@class"),
                                    _config) as ILogger;

            if (_logger == null) _logger = new NullLogger();

            _transformOutput = new FileTransformOutput(_logger);
            _context = new TransformContext(_config);

            _filters = new List<ITransformFilter>();

            XmlNode filterNode = _config.GetConfigSection("filters");
            if (filterNode != null)
            {
                foreach (XmlNode node in filterNode.ChildNodes)
                {
                    DictionaryConfiguration dict = new DictionaryConfiguration();
                    dict.Add("filter", node.OuterXml);
                    _filters.Add( CreateAndInit(node.SelectSingleNode("@assembly").Value, 
                                                node.SelectSingleNode("@class").Value,
                                                dict) as ITransformFilter); 
                }
            }

            Hashtable extensions = new Hashtable();
            extensions.Add(XsltExtension.EXTENSION_URN, new XsltExtension(_context));           

            if ((extraArgs.Count < 3) | (((extraArgs.Count - 3) % 2) != 0))
            {
                Console.WriteLine("PerfectStorm.CodeGen [-config=filename] model transform target [name value]*");
                Console.WriteLine("-config allows alternate configurations to be used in the same build process.");
                Console.WriteLine("model is an xml document.");
                Console.WriteLine("transform is (normally) an xslt document.");
                Console.WriteLine("file is the output.");
                Environment.Exit(1);
            }
            else
            {
                _logger.Log("Codegen Starting " + DateTime.Now.ToLongTimeString());

                string source = extraArgs[0];
                string transform = extraArgs[1];
                string target = extraArgs[2];
                string[] argList = new string[extraArgs.Count - 3];
                for (int i = 3; i < extraArgs.Count; i++)
                {
                    argList[i - 3] = extraArgs[i];
                }

                try
                {
                    TransformEngine engine = new TransformEngine(extensions, 
                                                                 _context, 
                                                                 _config, 
                                                                 _logger, 
                                                                 _transformOutput,
                                                                 _filters);
                    engine.ExecuteTransform(source, transform, target, argList);
                }
                catch (Exception e)
                {
                    Console.WriteLine("CodeGen failed creating {0}", target);
                    Console.WriteLine(e.Message);
                    if (e.InnerException != null)
                    {
                        // This is usually the useful one - line and column.
                        Console.WriteLine(e.InnerException.Message);
                    }
                    _logger.Log(string.Format("Codegen Failed processing {0} {1}", target, DateTime.Now.ToLongTimeString()));
                    Environment.Exit(2);
                }
                _logger.Log("Codegen Finished " + DateTime.Now.ToLongTimeString());
            }
        }
    }

}
