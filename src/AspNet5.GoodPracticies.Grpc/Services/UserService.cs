using System.Threading.Tasks;
using AspNet5.GoodPracticies.Grpc;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace src.AspNet5.GoodPracticies.Grpc.Services
{
    public class UserService : UserRPCService.UserRPCServiceBase
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public override async Task<UserInfoModel> GetUserInfo(GetUserInfoRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new UserInfoModel() { FirstName = "Joshep Edward", LastName = "Simas Almeida" });
        }
    }
}