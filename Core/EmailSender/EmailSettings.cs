using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EmailSender
{
    public class EmailSettings
    {
        public string ServerName { get; set; }
        public int PortNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; }
        public bool BodyIsHtml { get; set; }
    }
}
