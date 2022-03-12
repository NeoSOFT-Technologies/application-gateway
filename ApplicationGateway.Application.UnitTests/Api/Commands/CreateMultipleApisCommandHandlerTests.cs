using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
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
    public class CreateMultipleApisCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ISnapshotService> _snapshotService;
        private readonly Mock<ILogger<CreateMultipleApisCommandHandler>> _mockLogger;
        private readonly Mock<IApiRepository> _mockApiRepository;
        private readonly Mock<IApiService> _mockApiService;

        public CreateMultipleApisCommandHandlerTests()
        {

            _mockApiRepository = ApiRepositoryMocks.GetApiRepository();
            _mockApiService = ApiServiceMocks.GetApiService();
            _snapshotService = new Mock<ISnapshotService>();
            _mockLogger = new Mock<ILogger<CreateMultipleApisCommandHandler>>();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_create_Multiple_api()
        {
            var handler = new CreateMultipleApisCommandHandler(_mockApiRepository.Object, _snapshotService.Object, _mockApiService.Object, _mapper, _mockLogger.Object);
            var result = await handler.Handle(new CreateMultipleApisCommand()
            {
                APIs = new List<MultipleApiModel>() { { new MultipleApiModel() { Name = "Api1", ListenPath = "/apiListenPath/", TargetUrl = "http://localhost:3000" } }, { new MultipleApiModel() { Name = "Api2", ListenPath = "/apiListenPath2/", TargetUrl = "http://localhost:3001" } } }

            },
            CancellationToken.None);
            result.ShouldBeOfType<Response<CreateMultipleApisDto>>();
            var allApis = _mockApiRepository.Object.ListAllAsync().Result;
            allApis.Count().ShouldBe(4);
            result.ShouldNotBeNull();
        }
    }
}
