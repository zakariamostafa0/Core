using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ExceptionLog
{
    public enum LogType
    {
        File,
        Email
    }
    public class ExceptionLogFactory
    {
        
        private readonly IExceptionLog _logger;

        public ExceptionLogFactory(LogType logType, string parameter)
        {
            if (logType == LogType.File)
            {
                _logger = new FileExceptionLog(parameter);
            }
            else if (logType == LogType.Email)
            {
                _logger = new EmailExceptionLog(parameter);
            }
            else
            {
                throw new ArgumentException("Invalid log type");
            }
        }

        public string LogException(Exception exception)
        {
            
            return _logger.LogException(exception);
        }
    }
}
