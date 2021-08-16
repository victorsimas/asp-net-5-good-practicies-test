using System.Threading.Tasks;
using AspNet5.GoodPracticies.DTO.Data;
using AspNet5.GoodPracticies.Grpc;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace src.AspNet5.GoodPracticies.Grpc.Services
{
    public class UserService : UserRPCService.UserRPCServiceBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly UsersDBContext _context;

        public UserService(ILogger<UserService> logger, UsersDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public override async Task<UserInfoModel> GetUserInfo(GetUserInfoRequest request, ServerCallContext context)
        {
            UserDBModel user = await _context.Users.FindAsync(request.UserId);

            if (user is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User Not Found."));
            }

            return new UserInfoModel() { UserType = user.UserType, FirstName = user.FirstName, LastName = user.LastName, Age = user.Age };
        }
    }
}