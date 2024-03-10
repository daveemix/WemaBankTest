namespace AbiaPayCollectionMiddleware.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public ApiResponse(T data)
        {
            Success = true;
            Data = data;
            Message = "successful";
        }

        public ApiResponse(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
        }
    }

}
