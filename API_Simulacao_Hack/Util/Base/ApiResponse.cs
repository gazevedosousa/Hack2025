namespace API_Simulacao_Hack.Util.Base
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public T? Data { get; set; } = default;

        public string? ErrorMessage { get; set; }

        public static ApiResponse<T> Error(string errorMessage, int statusCode = StatusCodes.Status400BadRequest)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResponse<T> SuccessOK(T? data)
        {

            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status200OK,
                Data = data
            };
        }

        public static ApiResponse<T> SuccessOK(T data, int statusCode, string? errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Data = data,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResponse<T> SuccessCreated(T data)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status201Created,
                Data = data
            };
        }

        public static ApiResponse<T> NotFound(T? data)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Data = data
            };
        }


        public static ApiResponse<T> NotFound(T? data, string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Data = data,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResponse<T> BadRequest(T? data, string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Data = data,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResponse<T> UnprocessableEntity(T? data, string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Data = data,
                ErrorMessage = errorMessage
            };
        }
        public static ApiResponse<T> NoContent(T data, string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status204NoContent,
                Data = data,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResponse<T> NoContent( string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status204NoContent,
                ErrorMessage = errorMessage
            };
        }
    }
}
