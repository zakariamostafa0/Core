using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Core.ExceptionLog
{
    public class EmailExceptionLog : IExceptionLog
    {
        private readonly string _emailAddress;
        public EmailExceptionLog(string emailAddress) 
        {
            _emailAddress = emailAddress;
        }
        public string LogException(Exception exception)
        {
            try
            {
                var mailMessage = new MailMessage("zeko10191@gmail.com", _emailAddress)
                {
                    Subject = "Exception Occurred",
                    Body = $"Date is: [{DateTime.Now}]  Exception Type {exception.GetType().Name}\n" +
                           $"Message: {exception.Message}\n" +
                           $"Stack Trace: {exception.StackTrace}\n"
                };

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("zeko10191@gmail.com", "boqrehsndjgtlzgf"),
                    EnableSsl = true
                };
                smtpClient.Send(mailMessage);
                return "Unfortunately An error occurred. Please check your email for details.";
            }
            catch (Exception ex)
            {
                return $"Failed to send the exception email: {ex.Message}";
            }
        }
    }
}
