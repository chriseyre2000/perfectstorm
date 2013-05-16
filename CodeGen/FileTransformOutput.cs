using System;
using System.IO;
using System.Text;

namespace PerfectStorm.CodeGen
{
    public class FileTransformOutput : ITransformOutput
    {
        #region ITransformOutput Members

        private ILogger _logger;

        public FileTransformOutput(ILogger logger) : base()
        {
            _logger = logger;
        }

        public void WriteOutput(string target, System.IO.MemoryStream prevOutput, System.IO.MemoryStream output)
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

        #endregion
    }
}
