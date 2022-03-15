using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Profiles;
using ApplicationGateway.Application.UnitTests.Mocks;
using ApplicationGateway.Domain.Entities;
using ApplicationGateway.Domain.GatewayCommon;
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
    public class UpdateApiCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ISnapshotService> _snapshotService;
        private readonly Mock<ILogger<UpdateApiCommandHandler>> _mockLogger;
        private readonly Mock<IApiRepository> _mockApiRepository;
        private readonly Mock<IApiService> _mockApiService;

        public UpdateApiCommandHandlerTests()
        {
            _mockApiRepository = ApiRepositoryMocks.GetApiRepository();
            _mockApiService = ApiServiceMocks.GetApiService();
            _snapshotService = new Mock<ISnapshotService>();
            _mockLogger = new Mock<ILogger<UpdateApiCommandHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_Updated_Api()
        {
            var handler = new UpdateApiCommandHandler(_snapshotService.Object, _mockApiService.Object, _mockApiRepository.Object, _mapper, _mockLogger.Object);
            var ApiId = _mockApiService.Object.GetAllApisAsync().Result.FirstOrDefault().ApiId;

            var oldApi = await _mockApiService.Object.GetApiByIdAsync(ApiId);
            await handler.Handle(new UpdateApiCommand()
            {
                ApiId = ApiId,
                Name = "updatedApi",
                ListenPath = "/testpaths/",
                TargetUrl = "http://localhost:5001",
                RateLimit = new UpdateRateLimit() { Rate = 5, Per = 20 },
                Blacklist = new List<string> { "192.168.4.6", "125.365.548" },
                Whitelist = new List<string> { "192.168.4.6", "125.365.548" },
                VersioningInfo = new UpdateVersioningInfo() { Location = VersioningLocation.header, Key = "mykey2" },
                DefaultVersion = "Default2",
                Versions = new List<UpdateVersionModel> { new UpdateVersionModel() { Name = "versionName2", OverrideTarget = "overridetarget2" } },
                AuthType = "open",
                OpenidOptions = new UpdateOpenIdOptions() { Providers = new List<UpdateProvider> { new UpdateProvider() { Issuer = "issuer2", Client_ids = new List<UpdateClientPolicy> { new UpdateClientPolicy() { ClientId = "clientid2", Policy = "policy2" } } } } },
                LoadBalancingTargets = new List<string> { "target4", "target5", "target6" }
            }, CancellationToken.None);

            var allApis = await _mockApiService.Object.GetAllApisAsync();
            allApis.ShouldContain(oldApi);
            allApis.Count.ShouldBe(2);            
        }
    }
}
