using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace PerfectStorm.CodeGenLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class WriteOnceFilter : ITransformFilter, IPluginInit
    {
        private IConfiguration _config;

        #region ITransformFilter Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="xslt"></param>
        /// <param name="outputFilename"></param>
        /// <param name="data"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public bool BeforeTransform(XmlDocument model, XmlDocument xslt, string outputFilename, System.Collections.Hashtable data, params string[] arguments)
        {
            return true;
            //throw new Exception("The method or operation is not implemented.");
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
        public MemoryStream AfterTransform(XmlDocument model, XmlDocument xslt, MemoryStream priorOutput, System.IO.MemoryStream output, string outputFilename, System.Collections.Hashtable data, params string[] arguments)
        {
            return null;
        }

        #endregion

        #region IPluginInit Members

        public void Init(IConfiguration config)
        {
            _config = config;
        }

        #endregion
    }
}
