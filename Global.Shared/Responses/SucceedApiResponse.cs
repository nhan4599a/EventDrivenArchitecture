namespace Global.Shared.Responses
{
    public class SucceedApiResponse : ApiResponse
    {
        public SucceedApiResponse(string message) : base(true, message)
        {
        }
    }

    public class SucceedApiResponse<TData> : ApiResponse
    {
        public TData Data { get; private set; }

        public SucceedApiResponse(TData data, string message) : base(true, message)
        {
            Data = data;
        }
    }
}
