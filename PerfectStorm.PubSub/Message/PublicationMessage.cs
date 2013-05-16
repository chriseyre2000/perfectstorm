using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerfectStorm.PubSub.Message
{
    public class PublicationMessage : IPublicationMessage
    {
        public string Message
        {
            get; set;
        }

        public string Subject
        {
            get; set;
        }
    }
}
