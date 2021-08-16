using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using AspNet5.GoodPracticies.Grpc;
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
            _channel = GrpcChannel.ForAddress("http://localhost:5000");
            _serviceClient = new(_channel);
        }

        [HttpGet]
        public async Task<IActionResult> Get([Required] int userId)
        {
            return Ok(await _serviceClient.GetUserInfoAsync(new() { UserId = 123 }));
        }
    }
}
