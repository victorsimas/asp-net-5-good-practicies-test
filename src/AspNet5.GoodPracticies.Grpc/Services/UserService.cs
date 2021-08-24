using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNet5.GoodPracticies.DTO.Data;
using AspNet5.GoodPracticies.Grpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
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

        public override async Task<UserInfoModel> GetUserInfo(UserIdentityModel request, ServerCallContext context)
        {
            UserDBModel user = await _context.Users.FindAsync(request.UserId);

            if (user is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User Not Found."));
            }

            return new UserInfoModel() { UserId = user.UserId, UserType = user.UserType, FirstName = user.FirstName, LastName = user.LastName, Age = user.Age };
        }

        public override async Task GetManyUsersInfo(GetManyUsersInfoRequest request, IServerStreamWriter<UserInfoModel> responseStream, ServerCallContext context)
        {
            Stopwatch watch = new Stopwatch();

            request.Page -= 1;

            if (request.AsyncList)
            {
                watch.Start();

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
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Age = user.Age,
                            UserType = user.UserType
                        });
                    }

                    _logger.LogInformation($"Time of {watch.ElapsedMilliseconds}" );
                }
                else
                {
                    throw new RpcException(new (StatusCode.NotFound, $"It was not possible to find any users, Time of {watch.ElapsedMilliseconds}"));
                }

                watch.Reset();
            }
            else
            {
                watch.Start();

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
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Age = user.Age,
                            UserType = user.UserType
                        });
                    }

                    _logger.LogInformation($"Time of {watch.ElapsedMilliseconds}" );
                }
                else
                {
                    throw new RpcException(new (StatusCode.NotFound, $"It was not possible to find any users, Time of {watch.ElapsedMilliseconds}"));
                }

                watch.Reset();
            }
        }

        public override async Task<Empty> AddUser(UserInfoModel request, ServerCallContext context)
        {
            UserDBModel userToInsert = new()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                UserType = request.UserType
            };

            try 
            {
                await _context.Users.AddAsync(userToInsert);

                await _context.SaveChangesAsync();

                return new Empty();
            }
            catch (Exception)
            {
                throw new RpcException(new (StatusCode.FailedPrecondition, $"It was not possible to insert any user"));
            }
        }

        public override async Task<Empty> UpdateUser(UserInfoModel request, ServerCallContext context)
        {
            UserDBModel userToInsert = new(request.UserId)
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                UserType = request.UserType
            };

            try 
            {
                _context.Entry(userToInsert).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return new Empty();
            }
            catch (Exception)
            {
                throw new RpcException(new (StatusCode.FailedPrecondition, $"It was not possible to "));
            }
        }

        public override async Task<Empty> RemoveUser(UserIdentityModel request, ServerCallContext context)
        {
            try 
            {
                UserDBModel user = await _context.Users.FindAsync(request.UserId);

                if (user is not null)
                {
                    _context.Users.Remove(user);

                    await _context.SaveChangesAsync();

                    return new Empty();
                }
                
                throw new RpcException(new (StatusCode.NotFound, $"It was not possible to find any users"));
            }
            catch(RpcException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new RpcException(new (StatusCode.FailedPrecondition, $"It was not possible to "));
            }
        }
    }
}