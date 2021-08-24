using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using AspNet5.GoodPracticies.Grpc;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNet5.GoodPracticies.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly GrpcChannel _channel;
        private readonly UserRPCService.UserRPCServiceClient _serviceClient;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            _channel = GrpcChannel.ForAddress("http://localhost:6000");
            _serviceClient = new(_channel);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get([Required] int userId)
        {
            try
            {
                return Ok(await _serviceClient.GetUserInfoAsync(new() { UserId = userId }));
            }
            catch(RpcException ex)
            {
                return StatusCode(ex.ValidateRPCExceptionStatus());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersPaginated([Required][FromQuery] GetManyUsersInfoRequest request)
        {
            request.Page = request.Page == 0 ? 1 : request.Page;
            request.Quantity = request.Quantity == 0 ? 1 : request.Quantity;

            using AsyncServerStreamingCall<UserInfoModel> call = _serviceClient.GetManyUsersInfo(request);

            List<UserInfoModel> users = new();

            try
            {
                while (await call.ResponseStream.MoveNext())
                {
                    users.Add(call.ResponseStream.Current);
                }

                return Ok(users);
            }
            catch(RpcException ex)
            {
                return StatusCode(ex.ValidateRPCExceptionStatus());
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostNewUser([Required][FromBody] UserInfoModel user)
        {
            try
            {
                await _serviceClient.AddUserAsync(user);

                return Accepted();
            }
            catch(RpcException ex)
            {
                return StatusCode(ex.ValidateRPCExceptionStatus());
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo([Required][FromBody] UserInfoModel user)
        {
            try
            {
                await _serviceClient.UpdateUserAsync(user);

                return Accepted();
            }
            catch(RpcException ex)
            {
                return StatusCode(ex.ValidateRPCExceptionStatus());
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([Required][FromBody] UserIdentityModel user)
        {
            try
            {
                await _serviceClient.RemoveUserAsync(user);

                return Accepted();
            }
            catch(RpcException ex)
            {
                return StatusCode(ex.ValidateRPCExceptionStatus());
            }
        }
    }
}
