using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery;
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

namespace ApplicationGateway.Application.UnitTests.Gateway.Policy.Queries
{
    public class GetAllPoliciesQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IPolicyRepository> _mockPolicyRepository;
        private readonly Mock<ILogger<GetAllPoliciesQueryHandler>> _mockLogger;
        private readonly Mock<IPolicyService> _mockPolicyService;
        private readonly Mock<IApiService> _mockApiService;

        public GetAllPoliciesQueryHandlerTests()
        {
            _mockPolicyRepository = PolicyRepositoryMocks.GetPolicyRepository();
            _mockPolicyService = PolicyServiceMocks.GetPolicyService();
            _mockApiService = ApiServiceMocks.GetApiService();
            _mockLogger = new Mock<ILogger<GetAllPoliciesQueryHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_GetAllPolicies()
        {
            var handler = new GetAllPoliciesQueryHandler(_mockPolicyRepository.Object, _mapper, _mockLogger.Object, _mockPolicyService.Object, _mockApiService.Object);
            var query = new GetAllPoliciesQuery();
            query.searchParam = new();
            query.sortParam = new();
            var result = await handler.Handle(query, CancellationToken.None);
            var allPolicies = await _mockPolicyRepository.Object.ListAllAsync();
            result.ShouldBeOfType<PagedResponse<GetAllPoliciesDto>>();
            allPolicies.Count.ShouldBe(2);
        }
    }
}
