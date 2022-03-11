using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
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

namespace ApplicationGateway.Application.UnitTests.Api.Commands
{
    public class CreateApiCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ISnapshotService> _snapshotService;
        private readonly Mock<ILogger<CreateApiCommandHandler>> _mockLogger;
        private readonly Mock<IApiRepository> _mockApiRepository;
        private readonly Mock<IApiService> _mockApiService;

        public CreateApiCommandHandlerTests()
        {
            _mockApiRepository = ApiRepositoryMocks.GetApiRepository();
            _mockApiService = ApiServiceMocks.GetApiService();
            _snapshotService = new Mock<ISnapshotService>();
            _mockLogger = new Mock<ILogger<CreateApiCommandHandler>>();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_create_api()
        {
            var handler = new CreateApiCommandHandler(_snapshotService.Object, _mockApiService.Object, _mapper, _mockLogger.Object, _mockApiRepository.Object);
            var result = await handler.Handle(new CreateApiCommand() 
            { 
              Name = "myapi1",
              ListenPath = "/testpath1/",
              TargetUrl ="https://localhost:3011"
            }, 
            CancellationToken.None);
            result.ShouldBeOfType<Response<CreateApiDto>>();
            result.ShouldNotBeNull();
        }
    }


}
