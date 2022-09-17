namespace Global.Shared.Responses
{
    public abstract class ApiResponse
    {
        protected bool IsSucceedResponse { get; set; }

        protected string Message { get; set; } = default!;

        protected ApiResponse(bool isSucceedResponse, string message)
        {
            IsSucceedResponse = isSucceedResponse;
            Message = message;
        }
    }
}
