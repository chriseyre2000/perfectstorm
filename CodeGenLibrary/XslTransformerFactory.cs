using System;
using System.Collections.Generic;
using System.Text;

namespace PerfectStorm.CodeGenLibrary
{
    /// <summary>
    /// This is a singleton factory - but since I am only wrapping framework
    /// classes I will not be mocking these.
    /// </summary>
    public static class XslTransformerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transformerToken"></param>
        /// <returns></returns>
        public static IXlsTransformer Create(string transformerToken)
        {
            IXlsTransformer result = null;

            switch (transformerToken.ToUpper())
            {
                case "MONO": result = new XslTransformAdapter();
                    break;
                default: result = new XslCompiledTransformAdapter();
                    break;
            }

            return result;
        }
    }
}
