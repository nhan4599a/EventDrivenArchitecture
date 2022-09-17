using Global.Shared.Constants;
using Global.Shared.Responses;
using Infrastructure.Constants;

namespace Infrastructure.Exceptions
{
    public class RequestValidationFailedException : WebApplicationException
    {
        public override int ResponseCode => SharedConstants.HttpResponseCodes.BAD_REQUEST;

        public ValidationErrorCollection ValidationErrors { get; }

        public RequestValidationFailedException(ValidationErrorCollection validationErrors)
            : base(InfrastructureConstants.ErrorMessages.VALIDATION_FAILED)
        {
            ValidationErrors = validationErrors;
        }
    }
}
