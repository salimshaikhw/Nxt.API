using System;

namespace Nxt.Common.Exceptions
{
    [Serializable]
    public class ControllerException : NxtException
    {
        public ControllerException(string message, Exception innerException = null, ExceptionCodes exceptionCodes = ExceptionCodes.Default)
            : base($"Controller Exception: {message}", innerException, exceptionCodes)
        {
        }

        public ControllerException(string message, ExceptionCodes exceptionCodes = ExceptionCodes.Default)
            : base($"Controller Exception: {message}", null, exceptionCodes)
        {
        }
    }
}
