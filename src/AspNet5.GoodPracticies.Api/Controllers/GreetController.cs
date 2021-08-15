using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    public class GreetController : ControllerBase
    {
        private readonly ILogger<GreetController> _logger;

        public GreetController(ILogger<GreetController> logger)
        {
            _logger = logger;
        }

        [HttpGet("say-hello")]
        public async Task<IActionResult> Get([Required] string name)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Greeter.GreeterClient(channel);

            return Ok(await client.SayHelloAsync(new() { Name = name }));
        }
    }
}
