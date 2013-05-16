using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace PerfectStorm.CodeGen
{
    /// <summary>
    /// The purpose of this filter is to remove a unicode header.
    /// </summary>
    public class SupressUnicodePrefixFilter : ITransformFilter
    {
        #region ITransformFilter Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="xslt"></param>
        /// <param name="priorOutput"></param>
        /// <param name="output"></param>
        /// <param name="outputFilename"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public MemoryStream AfterTransform(XmlDocument model, XmlDocument xslt, MemoryStream priorOutput, MemoryStream output, string outputFilename, Hashtable data)
        {
            MemoryStream result = output;

            if (output.Length > 3)
            {
                Boolean skipHeader = true;
                Byte[] check = { 239, 187, 191 };
                for (int i = 0; i < 3; i++)
                {
                    if (output.ReadByte() != check[i])
                    {
                        skipHeader = false;
                        break;
                    }
                }

                if (skipHeader)
                {
                    MemoryStream ms2 = new MemoryStream(output.GetBuffer(), 3, (int)output.Length - 3);
                    result = ms2;
                }
            }
            result.Position = 0;
            return result;
        }

        #endregion
    }
}
