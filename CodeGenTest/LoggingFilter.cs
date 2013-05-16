using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using PerfectStorm.CodeGenLibrary;

namespace PerfectStorm.CodeGenTest
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggingFilter : ITransformFilter
    {
        private ILogger _logger = null;

        public LoggingFilter(ILogger logger)
            : base()
        {
            _logger = logger;
        }

        #region ITransformFilter Members

        public bool BeforeTransform(XmlDocument model, XmlDocument xslt, string outputFilename, Hashtable data, params string[] arguments)
        {
            _logger.Log("LoggingFilter: BeforeTransform called");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="xslt"></param>
        /// <param name="priorOutput"></param>
        /// <param name="output"></param>
        /// <param name="outputFilename"></param>
        /// <param name="data"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public MemoryStream AfterTransform(XmlDocument model, XmlDocument xslt, MemoryStream priorOutput, MemoryStream output, string outputFilename, Hashtable data, params string[] arguments)
        {
            _logger.Log("LoggingFilter: AfterTransform called");
            return null;
        }

        #endregion
    }
}
