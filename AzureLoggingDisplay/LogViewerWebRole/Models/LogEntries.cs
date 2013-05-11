using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogViewerWebRole.Models
{
    public class LogItem
    {
        public int Level { get; set; }
        public string RoleInstance { get; set; }
        public string Message { get; set; }
        public string PartitionKey { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    public interface ILogEntries 
    {
        List<LogItem> Log { get; set; }
    }

    public class LogEntries : ILogEntries
    {
        public LogEntries() 
        {
            Log = new List<LogItem>();
        }

        public List<LogItem> Log { get; set; }
    }
}