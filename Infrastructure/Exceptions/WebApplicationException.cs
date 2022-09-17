using Infrastructure.Constants;
using System;

namespace Infrastructure.Exceptions
{
    public abstract class WebApplicationException : Exception
    {
        public abstract int ResponseCode { get; }

        public virtual string OutputMessage { get; protected set; } = InfrastructureConstants.ErrorMessages.UNKNOWN_ERROR;

        protected WebApplicationException(string errorMessage, Exception? innerException = null)
            : base(errorMessage, innerException) { }

        internal virtual ApplicationExceptionDetail GetExceptionDetail()
        {
            return new ApplicationExceptionDetail(OutputMessage);
        }
    }

    internal class ApplicationExceptionDetail
    {
        public string Message { get; } = default!;

        public ApplicationExceptionDetail(string message)
        {
            Message = message;
        }
    }
}
