using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EmailSender.Services
{
    public interface IMailSender
    {
        void GetConfiguration(string emailConfigFilePath);
        void SendEmail(MessageData messageData);
    }
}
