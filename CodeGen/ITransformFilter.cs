using System;
using System.Collections;
using System.Xml;
using System.IO;

namespace PerfectStorm.CodeGen
{
    /// <summary>
    /// This is used to allow customisation of the output.
    /// It is intended that the filters be chained together and run in the order registered.
    /// 
    /// </summary>
    /// <remarks>Currently there is no Before Transform as I have yet to work out the required signature</remarks>
    interface ITransformFilter
    {
        /// <summary>
        /// This allows changes to be made to the output after the transformation has 
        /// been made.
        /// If the method returns null then the existing output will be maintained.
        /// </summary>
        /// <param name="model">A copy of the model document.</param>
        /// <param name="xslt">A copy of the transform document</param>
        /// <param name="priorOutput">
        /// If the file already existed then a copy of the prior version will be supplied.
        /// If the file did not previously exist this will be null.
        /// </param>
        /// <param name="output">
        /// This is the existing output.
        /// </param>
        /// <param name="outputFilename"></param>
        /// <param name="data">A Hashtable so that the transforms may communicate between each other.</param>
        /// <returns></returns>
        MemoryStream AfterTransform(XmlDocument model, XmlDocument xslt, MemoryStream priorOutput, MemoryStream output, string outputFilename, Hashtable data);
    }
}
