using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace PerfectStorm.CodeGen
{
    /// <summary>
    /// This is the engine that performs the transformation.
    /// </summary>
    public class TransformEngine
    {
        private Hashtable _extensions = null;
        private TransformContext _context = null;
        private IConfiguration _config = null;
        private ILogger _logger = null;
        private ITransformOutput _transformOutput = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="context"></param>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        public TransformEngine(Hashtable extensions, 
                               TransformContext context,
                               IConfiguration config,
                               ILogger logger,
                               ITransformOutput transformOutput)
            : base()
        {
            _config = config;
            _extensions = extensions;
            _context = context;
            _logger = logger;
            _transformOutput = transformOutput; 
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="transform"></param>
        /// <param name="target"></param>
        /// <param name="arguments"></param>
        public void ExecuteTransform(string source, string transform, string target, params string[] arguments)
        {
            _context.GeneratedFile = target;
            string transformerToken = _config.GetConfigData("transform", "@transformAdapter");

            IXlsTransformer xTransform = XslTransformerFactory.Create(transformerToken);
            xTransform.Load(transform);

            XsltArgumentList args = CreateXslArguments(arguments);
                        
            // Add the extension libraries
            if ((_extensions != null) && (_extensions.Count > 0))
            {
                foreach (string key in _extensions.Keys)
                {
                    args.AddExtensionObject(key, _extensions[key]);
                }
            }

            MemoryStream ms = new MemoryStream();
            xTransform.Transform(source, args, ms);

            ms.Position = 0;

            // This is the first use of a filter - the next step is to make them
            // generalised and chain them.
            if (_config.HasConfigItem("output", "SuppressUnicodePrefix"))
            {
                ITransformFilter filter = new SupressUnicodePrefixFilter();
                ms = filter.AfterTransform(null, null, null, ms, target, null);
            }
            _transformOutput.WriteOutput(target, null, ms);

            //TODO: Need to add logging details to allow file change to be detected.
        }


        /// <summary>
        /// This constructs the parameter list from the supplied arguments.
        /// It is assumed that these come in name value pairs.
        /// A stray parameter name will result in an empty string value.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static XsltArgumentList CreateXslArguments(string[] arguments)
        {
            string paramName = null;

            XsltArgumentList args = new XsltArgumentList();
            if ((arguments != null) && (arguments.Length > 0))
            {
                foreach (string param in arguments)
                {
                    if (paramName == null)
                    {
                        paramName = param;
                    }
                    else
                    {
                        args.AddParam(paramName, "", param);
                        paramName = null;
                    }
                }

                if (paramName != null)
                {
                    args.AddParam(paramName, "", "");
                }
            }
            return args;
        }

    }

}
