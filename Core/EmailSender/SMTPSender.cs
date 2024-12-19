using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Core.EmailSender.Services;
using Core.Encryption;
using Core.ExceptionLog;
namespace Core.EmailSender
{
    public class SMTPSender : IMailSender
    {
        private const string FileExtension = ".STD";
        private readonly GenericDataHandler<EmailSettings> _dataHandler
            = new GenericDataHandler<EmailSettings>();
        private EmailSettings _emailSettings;

        public void GetConfiguration(string emailConfigFilePath)
        {
            string fullPath = emailConfigFilePath + FileExtension;
            _emailSettings = _dataHandler.ReadObjectData(fullPath);

            if (_emailSettings == null)
            {
                throw new InvalidOperationException("Can't loading the data!");
            }
            Console.WriteLine("The Configuration successfully loaded");
        }

        public void SaveConfiguration(string emailConfigFilePath, EmailSettings emailSettings)
        {
            string fullPath = emailConfigFilePath + FileExtension;
            _dataHandler.SaveObjectData(fullPath, emailSettings);
            Console.WriteLine("The Configuration has been saved ");
        }

        public void SendEmail(MessageData messageData)
        {
            if (_emailSettings == null)
            {
                throw new InvalidOperationException("Email settings not found!.");
            }

            //Initialize Host Name and its Port Number
            using (var smtpClient = new SmtpClient(_emailSettings.ServerName, _emailSettings.PortNumber))
            {
                //Authorization the email and passord 
                smtpClient.Credentials = new NetworkCredential(_emailSettings.EmailAddress, _emailSettings.Password);
                smtpClient.EnableSsl = _emailSettings.UseSSL;

                //Initialize the message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.EmailAddress),
                    Subject = messageData.Subject,
                    Body = messageData.Content,
                    IsBodyHtml = _emailSettings.BodyIsHtml
                };

                foreach (var email in messageData.ToEmailAddresses)
                    mailMessage.To.Add(email);
                foreach (var email in messageData.CcEmailAddresses)
                    mailMessage.CC.Add(email);

                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    var exceptionLogger = new EmailExceptionLog("admin@yourdomain.com");
                    var logResult = exceptionLogger.LogException(ex);

                    Console.WriteLine($"Error sending email: {logResult}");
                }
            }
        }
    }
}
