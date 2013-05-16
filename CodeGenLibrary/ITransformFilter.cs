using System;
using System.Collections;
using System.Xml;
using System.IO;

namespace PerfectStorm.CodeGenLibrary
{
    /// <summary>
    /// This is used to allow customisation of the output.
    /// It is intended that the filters be chained together and run in the order registered.
    /// 
    /// </summary>
    public interface ITransformFilter
    {
        /// <summary>
        /// This allows changes to be made to the transform and the input.
        /// It may also be used to prevent the output from being written.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="xslt"></param>
        /// <param name="outputFilename"></param>
        /// <param name="data"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        bool BeforeTransform(XmlDocument model, XmlDocument xslt, string outputFilename, Hashtable data, params string[] arguments); 
        
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
        MemoryStream AfterTransform(XmlDocument model, XmlDocument xslt, MemoryStream priorOutput, MemoryStream output, string outputFilename, Hashtable data, params string[] arguments);
    }
}
