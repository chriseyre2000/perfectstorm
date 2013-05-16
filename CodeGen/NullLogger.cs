using System;
using System.Collections.Generic;
using System.Text;

namespace PerfectStorm.CodeGen
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
