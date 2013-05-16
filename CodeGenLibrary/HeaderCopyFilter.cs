using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;

namespace PerfectStorm.CodeGenLibrary
{
    /// <summary>
    /// The idea of this is that a header block can be carried
    /// forward from one file to the next.
    /// The start and end markers are copied from th old to the new.
    /// </summary>
    public class HeaderCopyFilter : ITransformFilter, IPluginInit
    {
        enum FileState { fsBefore, fsDuring, fsAfter };

        private string _startMarker = "/***";
        private string _endMarker = "***/";

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
            StringBuilder headerBlock = new StringBuilder();
            MemoryStream result = output;

            if ((priorOutput != null) && (output != null))
            {
                FileState state = FileState.fsBefore;
                string line = null;
                priorOutput.Position = 0;
                StreamReader priorReader = new StreamReader(priorOutput);
                line = priorReader.ReadLine();
                while (line != null)
                {
                    if ((state == FileState.fsBefore) && (line.StartsWith(_startMarker)))
                    {
                        state = FileState.fsDuring;
                        headerBlock.AppendLine(line);
                    }
                    else if ((state == FileState.fsDuring) && (line.EndsWith(_endMarker)))
                    {
                        state = FileState.fsAfter;
                        headerBlock.AppendLine(line);
                        break;
                    }
                    else if (state == FileState.fsDuring)
                    {
                        headerBlock.AppendLine(line);
                    }
                    
                    line = priorReader.ReadLine();
                }
                result = new MemoryStream();

                output.Position = 0;
                StreamReader outputReader = new StreamReader(output);
                StreamWriter outputWriter = new StreamWriter(result);

                state = FileState.fsBefore;

                line = outputReader.ReadLine();
                while (line != null)
                {
                    if ((state == FileState.fsBefore) && (line.StartsWith(_startMarker)))
                    {
                        state = FileState.fsDuring;
                    }
                    else if (state == FileState.fsBefore)
                    {
                        outputWriter.WriteLine(line);
                    }
                    else if ((state == FileState.fsDuring) && (line.EndsWith(_endMarker)))
                    {
                        state = FileState.fsAfter;
                        outputWriter.Write(headerBlock.ToString());
                        line = outputReader.ReadToEnd();
                        outputWriter.Write(line);
                        break;
                    }
                    line = outputReader.ReadLine();
                }
                outputWriter.Flush();
            }
            return result;
        }

        #endregion

        #region IPluginInit Members

        public void Init(IConfiguration config)
        {
            string startMarker = config.GetConfigData("filter", "@startMarker");
            string endMarker = config.GetConfigData("filter", "@endMarker");
            if ((startMarker.Length > 0) && (endMarker.Length > 0))
            {
                _startMarker = startMarker;
                _endMarker = endMarker;
            }
        }

        #endregion
    }
}
