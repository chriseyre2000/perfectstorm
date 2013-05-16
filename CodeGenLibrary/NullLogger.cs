using System;
using System.Collections.Generic;
using System.Text;

namespace PerfectStorm.CodeGenLibrary
{
    public class NullLogger : ILogger
    {
        #region ILogger Members

        public void Log(string message)
        {
            // Explicitly does nothing.
        }

        #endregion
    }
}
