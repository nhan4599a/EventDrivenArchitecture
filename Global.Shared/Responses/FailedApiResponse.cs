namespace Global.Shared.Responses
{
    public class FailedApiResponse : ApiResponse
    {
        public FailedApiResponse(string message) : base(false, message)
        {
        }
    }
}
