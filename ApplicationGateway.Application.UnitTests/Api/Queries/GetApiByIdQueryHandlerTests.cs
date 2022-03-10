using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Features.Api.Queries.GetApiByIdQuery;
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

namespace ApplicationGateway.Application.UnitTests.Gateway.Api.Queries
{
    public class GetApiByIdQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IApiService> _mockApiService;
        private readonly Mock<ILogger<GetApiByIdQueryHandler>> _mockLogger;

        public GetApiByIdQueryHandlerTests()
        {
            _mockApiService = ApiServiceMocks.GetApiService();
            _mockLogger = new Mock<ILogger<GetApiByIdQueryHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_GetAPIById()
        {
            var ApiId = _mockApiService.Object.GetAllApisAsync().Result.FirstOrDefault().ApiId;
            var handler = new GetApiByIdQueryHandler(_mockApiService.Object, _mapper, _mockLogger.Object);

            var result = await handler.Handle(new GetApiByIdQuery() { ApiId= ApiId }, CancellationToken.None);

            result.ShouldBeOfType<Response<GetApiByIdDto>>();


        }
    }
}
