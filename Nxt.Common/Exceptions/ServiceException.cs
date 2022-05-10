using System;

namespace Nxt.Common.Exceptions
{
    [Serializable]
    public class ServiceException : NxtException
    {
        public ServiceException(string message, Exception innerException = null, ExceptionCodes exceptionCodes = ExceptionCodes.Default)
            : base($"Service Exception: {message}", innerException, exceptionCodes)
        {
        }

        public ServiceException(string message, ExceptionCodes exceptionCodes = ExceptionCodes.Default)
           : base($"Service Exception: {message}", null, exceptionCodes)
        {
        }
    }
}
