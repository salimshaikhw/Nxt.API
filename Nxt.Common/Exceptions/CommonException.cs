using System;

namespace Nxt.Common.Exceptions
{
    [Serializable]
    public class CommonException : NxtException
    {
        public CommonException(string message, Exception innerException = null, ExceptionCodes exceptionCodes = ExceptionCodes.Default)
            : base($"CommonException: {message}", innerException, exceptionCodes)
        {
        }
    }
}
