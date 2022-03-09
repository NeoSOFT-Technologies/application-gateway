﻿using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery;
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

namespace ApplicationGateway.Application.UnitTests.Gateway.Api.Commands
{
    public class GetAllApisQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IApiService> _mockApiService;
        private readonly Mock<ILogger<GetAllApisQueryHandler>> _mockLogger;

        public GetAllApisQueryHandlerTests()
        {
            _mockApiService = ApiServiceMocks.GetApiService();
            _mockLogger = new Mock<ILogger<GetAllApisQueryHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_GetAllAPI()
        {
            var handler = new GetAllApisQueryHandler(_mockApiService.Object,_mapper,_mockLogger.Object);

            var result = await handler.Handle(new GetAllApisQuery(),CancellationToken.None);

            result.ShouldBeOfType<Response<GetAllApisDto>>();

        
        }

    }
}
