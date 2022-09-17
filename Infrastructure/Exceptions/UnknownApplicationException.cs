using Global.Shared.Constants;
using System;

namespace Infrastructure.Exceptions
{
    public class UnknownApplicationException : WebApplicationException
    {
        public override int ResponseCode => SharedConstants.HttpResponseCodes.INTERNAL_SERVER_ERROR;

        public UnknownApplicationException(Exception exception) : base(exception.Message, exception)
        { }
    }
}
