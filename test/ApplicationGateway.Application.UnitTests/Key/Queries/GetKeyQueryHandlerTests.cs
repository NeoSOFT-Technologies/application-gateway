using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Features.Key.Queries.GetKey;
using ApplicationGateway.Application.Profiles;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Application.UnitTests.Mocks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationGateway.Application.UnitTests.Key.Queries
{
    public class GetKeyQueryHandlerTests
    {
       
        private readonly Mock<IKeyService> _mockKeyService;
        private readonly Mock<ILogger<GetKeyQueryHandler>> _mockLogger;

        public GetKeyQueryHandlerTests()
        {
            _mockKeyService = KeyServiceMocks.GetKeyService();
            _mockLogger = new Mock<ILogger<GetKeyQueryHandler>>();
           
        }

        [Fact]
        public async Task Handle_GetKey()
        {
            var KeyId = "KeyId2";

            var handler = new GetKeyQueryHandler(_mockLogger.Object, _mockKeyService.Object);

            var result = await handler.Handle(new GetKeyQuery() { keyId= KeyId }, CancellationToken.None);

            result.ShouldBeOfType<Response<Domain.GatewayCommon.Key>>();


        }
    }
}
