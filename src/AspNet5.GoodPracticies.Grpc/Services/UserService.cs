using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNet5.GoodPracticies.DTO.Data;
using AspNet5.GoodPracticies.Grpc;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace src.AspNet5.GoodPracticies.Grpc.Services
{
    public class UserService : UserRPCService.UserRPCServiceBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly UsersDBContext _context;
        private static readonly Stopwatch _watch = new Stopwatch();

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

        public override async Task GetManyUsersInfo(GetManyUsersInfoRequest request, IServerStreamWriter<UserInfoModel> responseStream, ServerCallContext context)
        {
            request.Page -= 1;

            if (request.AsyncList)
            {
                _watch.Start();

                IAsyncEnumerable<UserDBModel> users = _context.Users
                    .Skip(request.Quantity * request.Page)
                        .Take(request.Quantity)
                            .AsAsyncEnumerable();
                
                if (users is not null)
                {
                    await foreach(UserDBModel user in users)
                    {
                        await responseStream.WriteAsync(new UserInfoModel()
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Age = user.Age,
                            UserType = user.UserType
                        });
                    }

                    _logger.LogInformation($"Time of {_watch.ElapsedMilliseconds}" );
                }
                else
                {
                    throw new RpcException(new (StatusCode.NotFound, $"It was not possible to find any users, Time of {_watch.ElapsedMilliseconds}"));
                }

                _watch.Reset();
            }
            else
            {
                _watch.Start();

                IEnumerable<UserDBModel> users = await _context.Users
                    .Skip(request.Quantity * request.Page)
                        .Take(request.Quantity)
                            .ToListAsync();
                
                if (users is not null)
                {
                    foreach(UserDBModel user in users)
                    {
                        await responseStream.WriteAsync(new UserInfoModel()
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Age = user.Age,
                            UserType = user.UserType
                        });
                    }

                    _logger.LogInformation($"Time of {_watch.ElapsedMilliseconds}" );
                }
                else
                {
                    throw new RpcException(new (StatusCode.NotFound, $"It was not possible to find any users, Time of {_watch.ElapsedMilliseconds}"));
                }

                _watch.Reset();
            }
        }
    }
}