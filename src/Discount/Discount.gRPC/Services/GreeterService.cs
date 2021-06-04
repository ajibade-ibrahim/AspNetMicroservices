using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.gRPC
{
    public class GreeterService : Greeter.GreeterBase
    {
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        private readonly ILogger<GreeterService> _logger;

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(
                new HelloReply
                {
                    Message = "Hello " + request.Name
                });
        }
    }
}