using ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper;
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
        private readonly Mock<ILogger<GetKeyQueryHandler>> _mockLogger;

        public GetKeyQueryHandlerTests()
        {
            _mockKeyService = KeyServiceMocks.GetKeyService();
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
            var handler = new GetKeyQueryHandler(_mockLogger.Object, _mapper, _mockKeyService.Object);

            var result = await handler.Handle(new GetKeyQuery(), CancellationToken.None);

            result.ShouldBeOfType<Response<Domain.Entities.Key>>();


        }
    }
}
