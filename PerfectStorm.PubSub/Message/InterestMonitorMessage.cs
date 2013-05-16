using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerfectStorm.PubSub.Message
{
    public class InterestMonitorMessage : IInterestMonitorMessage
    {
        public int Count
        {
            get; set;
        }

        public string Subject
        {
            get; set;
        }
    }
}
