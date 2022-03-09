using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery;
using ApplicationGateway.Application.Profiles;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Application.UnitTests.Mocks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationGateway.Application.UnitTests.Gateway.Policy.Queries
{
    public class GetAllPoliciesQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IPolicyService> _mockPolicyService;
        private readonly Mock<ILogger<GetAllPoliciesQueryHandler>> _mockLogger;

        public GetAllPoliciesQueryHandlerTests()
        {
            _mockPolicyService = PolicyServiceMocks.GetPolicyService();
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
            var handler = new GetAllPoliciesQueryHandler(_mockPolicyService.Object, _mapper, _mockLogger.Object);

            var result = await handler.Handle(new GetAllPoliciesQuery(), CancellationToken.None);

            result.ShouldBeOfType<Response<List<GetAllPoliciesDto>>>();


        }
    }
}
