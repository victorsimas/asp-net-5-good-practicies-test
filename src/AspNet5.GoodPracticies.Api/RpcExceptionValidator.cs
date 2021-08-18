using static Microsoft.AspNetCore.Http.StatusCodes;
using Grpc.Core;

namespace AspNet5.GoodPracticies.Api
{
    public static class RpcExceptionValidator
    {
        public static int ValidateRPCExceptionStatus(this RpcException exception)
        {
            return exception.StatusCode switch
            {
                StatusCode.NotFound => Status404NotFound,
                _ => Status500InternalServerError
            };
        }
    }
}