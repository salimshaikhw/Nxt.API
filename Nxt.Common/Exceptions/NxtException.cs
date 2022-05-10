using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Nxt.Common.Exceptions
{
    [Serializable]
    public abstract class NxtException : Exception
    {
        public Guid ReferenceId { get; }
        public ExceptionCodes ExceptionCode { get; set; }
        public NxtException(string message, Exception innerException = null, ExceptionCodes exceptionCode = ExceptionCodes.Default)
            : base(message, innerException)
        {
            ExceptionCode = exceptionCode;
            ReferenceId = (innerException as NxtException)?.ReferenceId ?? Guid.NewGuid();
        }

        protected NxtException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
            ExceptionCode = (ExceptionCodes)serializationInfo.GetInt32("ExceptionCode");
            ReferenceId = Guid.Parse(serializationInfo.GetString("ReferenceId"));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ExceptionCode", ExceptionCode);
            info.AddValue("ReferenceId", ReferenceId);
        }
    }
}
