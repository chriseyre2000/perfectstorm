using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace PerfectStorm.CodeGenLibrary
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
        private List<ITransformFilter> _filters = null;

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
                               ITransformOutput transformOutput,
                               List<ITransformFilter> filters)
            : base()
        {
            _config = config;
            _extensions = extensions;
            _context = context;
            _logger = logger;
            _transformOutput = transformOutput;
            _filters = filters;
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
            Hashtable data = new Hashtable();
            
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

            XmlDocument xSourceDoc = new XmlDocument();
            xSourceDoc.Load(source);

            XmlDocument xTransformDoc = new XmlDocument();
            xTransformDoc.Load(transform);

            MemoryStream msTarget = null;

            if (File.Exists(target))
            {
                msTarget = new MemoryStream();
                FileStream fsTarget = new FileStream(target, FileMode.Open);
                StreamReader reader = new StreamReader(fsTarget);
                StreamWriter writer = new StreamWriter(msTarget);
                writer.Write(reader.ReadToEnd());
                writer.Flush();
                fsTarget.Close();
                msTarget.Position = 0;
            }

            bool MayWriteOutput = true;

            if (_filters != null)
            {
                foreach (ITransformFilter filter in _filters)
                {
                    MayWriteOutput = MayWriteOutput &&
                        filter.BeforeTransform(xSourceDoc, xTransformDoc, target, data, arguments);
                }
            }

            MemoryStream msOutput = new MemoryStream();
            xTransform.Transform(source, args, msOutput);

            msOutput.Position = 0;

            // This allows for chained filters.
            if (_filters != null)
            {
                foreach (ITransformFilter filter in _filters)
                {
                    MemoryStream newMs = filter.AfterTransform(xSourceDoc, xTransformDoc, msTarget, msOutput, target, data, arguments);
                    if (newMs != null)
                    {
                        msOutput = newMs;
                    }
                    msOutput.Position = 0;
                }            
            }

            if (MayWriteOutput)
            {
                _transformOutput.WriteOutput(target, msTarget, msOutput);
            }
            else
            {
                _logger.Log(string.Format("Transform of {0} prevented by filter.", target));
            }

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
