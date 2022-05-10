using System;

namespace Nxt.Common.Exceptions
{
    [Serializable]
    public class RepositoryException : NxtException
    {
        public RepositoryException(string message, Exception innerException = null, ExceptionCodes exceptionCodes = ExceptionCodes.Default)
            : base($"RepositoryException: {message}", innerException, exceptionCodes)
        {
        }
    }
}
