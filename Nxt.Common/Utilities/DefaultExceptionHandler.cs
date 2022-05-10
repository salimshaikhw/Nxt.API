using Nxt.Common.Exceptions;
using System;

namespace Nxt.Common.Utilities
{
    public static class DefaultExceptionHandler
    {
        public static string DefaultErrorMessage { get; set; } = "An error occured while performing requested operation.";

        static DefaultExceptionHandler()
        {

        }
        public static NxtException HandleException<TTargetException>(Exception exception, string message = null, ExceptionCodes exceptionCode = ExceptionCodes.Default) where TTargetException : NxtException
        {
            if (exception is AggregateException && exception.InnerException != null)
            {
                return HandleException<TTargetException>(exception.InnerException, message ?? DefaultErrorMessage, exceptionCode);
            }

            if (exception is NxtException nextEnsignException)
            {
                return nextEnsignException;
            }

            return (TTargetException)Activator.CreateInstance(typeof(TTargetException), message ?? DefaultErrorMessage, exception, exceptionCode);
        }
    }
}
