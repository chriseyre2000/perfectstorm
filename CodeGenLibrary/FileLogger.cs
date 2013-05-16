using System;
using System.IO;

namespace PerfectStorm.CodeGenLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class FileLogger : ILogger, IPluginInit
    {
        IConfiguration _config;
        string _fileName = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public FileLogger(IConfiguration config) : base()
        {
            Init(config);
        }
        /// <summary>
        /// 
        /// </summary>
        public FileLogger() : base() { }

        /// <summary>
        /// 
        /// </summary>
        public string Filename { set { _fileName = value; } }

        #region ILogger Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            if (_fileName != null)
            {
                try
                {
                    File.AppendAllText(_fileName, message + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    // Where else is logging code going to log too?
                    Console.WriteLine("Error during logging: " + ex.ToString());
                }
            }
        }

        #endregion

        #region IPluginInit Members

        public void Init(IConfiguration config)
        {
            _config = config;
            _fileName = _config.GetConfigData("logging", "@filename");
        }

        #endregion
    }
}
