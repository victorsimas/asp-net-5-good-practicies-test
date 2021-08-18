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
            return Ok(await _serviceClient.GetUserInfoAsync(new() { UserId = userId }));
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersPaginated([Required][FromQuery] GetManyUsersInfoRequest request)
        {
            request.Page = request.Page == 0 ? 1 : request.Page;
            request.Qtde = request.Qtde == 0 ? 1 : request.Qtde;

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
    }
}
