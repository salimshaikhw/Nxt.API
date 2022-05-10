using Nxt.Common.Exceptions;
using Nxt.Common.Utilities;
using System;

namespace Nxt.Services
{
    public abstract class Service
    {
        protected NxtException HandleException(Exception exception, string message = null, ExceptionCodes exceptionCode = ExceptionCodes.Default)
        {
            return DefaultExceptionHandler.HandleException<ServiceException>(exception, message, exceptionCode);
        }
    }
}
