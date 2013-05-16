using System;
using System.IO;
using System.Text;

namespace PerfectStorm.CodeGenLibrary
{
    public class FileTransformOutput : ITransformOutput
    {
        #region ITransformOutput Members

        private ILogger _logger;

        public FileTransformOutput(ILogger logger) : base()
        {
            _logger = logger;
        }

        public void WriteOutput(string target, MemoryStream prevOutput, MemoryStream output)
        {
            if (OutputChanged(prevOutput, output))
            {
                if (File.Exists(target))
                {
                    //TODO: Need to add hooks for smart filtering of headers
                    File.SetAttributes(target, FileAttributes.Normal);
                    FileStream Writer = new FileStream(target, FileMode.Truncate);
                    output.WriteTo(Writer);
                    Writer.Flush();
                    Writer.Close();
                    _logger.Log("Created: " + target);
                }
                else
                {
                    FileStream Writer = new FileStream(target, FileMode.Create);
                    output.WriteTo(Writer);
                    Writer.Flush();
                    Writer.Close();
                    _logger.Log("Regenerated: " + target);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldMs"></param>
        /// <param name="newMs"></param>
        /// <returns></returns>
        private bool OutputChanged(MemoryStream oldMs, MemoryStream newMs)
        {
            // If the old did not exist then feel free to write the new.
            if (oldMs == null) return true;

            // If the old is different length to the new don't do a detailed compare.
            if (oldMs.Length != newMs.Length) return true;

            StreamReader oldReader = new StreamReader(oldMs);
            StreamReader newReader = new StreamReader(newMs);
            if (oldReader.ReadToEnd() != newReader.ReadToEnd()) return true;

            return false;
        }


        #endregion
    }
}
