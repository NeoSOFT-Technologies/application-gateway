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
        private readonly IMapper _mapper;
        private readonly Mock<IKeyService> _mockKeyService;
        private readonly Mock<IApiService> _mockApiService;
        private readonly Mock<IPolicyService> _mockPolicyService;
        private readonly Mock<ILogger<GetKeyQueryHandler>> _mockLogger;

        public GetKeyQueryHandlerTests()
        {
            _mockKeyService = KeyServiceMocks.GetKeyService();
            _mockApiService = ApiServiceMocks.GetApiService();
            _mockPolicyService = PolicyServiceMocks.GetPolicyService();
            _mockLogger = new Mock<ILogger<GetKeyQueryHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_GetKey()
        {
            var KeyId = "KeyId2";

            var handler = new GetKeyQueryHandler(_mockLogger.Object, _mockKeyService.Object, _mapper, _mockApiService.Object, _mockPolicyService.Object);

            var result = await handler.Handle(new GetKeyQuery() { keyId= KeyId }, CancellationToken.None);

            result.ShouldBeOfType<Response<GetKeyDto>>();
        }
    }
}
