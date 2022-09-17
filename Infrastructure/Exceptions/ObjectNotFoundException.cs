using Global.Shared.Constants;

namespace Infrastructure.Exceptions
{
    public class ObjectNotFoundException<TObject> : WebApplicationException
    {
        public override int ResponseCode => SharedConstants.HttpResponseCodes.NOT_FOUND;

        public ObjectNotFoundException() : base($"{typeof(TObject).Name} not found!")
        {
            OutputMessage = Message;
        }
    }
}
