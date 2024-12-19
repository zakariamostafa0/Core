using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EmailSender
{
    public class MessageData
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<string> ToEmailAddresses { get; set; } = new List<string>();
        public List<string> CcEmailAddresses { get; set; } = new List<string>();
    }
}
