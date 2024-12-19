using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ExceptionLog
{
    public interface IExceptionLog
    {
        string LogException(Exception exception);
    }
}
