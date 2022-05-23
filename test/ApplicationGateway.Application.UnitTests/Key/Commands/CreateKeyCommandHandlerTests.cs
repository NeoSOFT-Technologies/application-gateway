using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand;
using ApplicationGateway.Application.Profiles;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Application.UnitTests.Mocks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationGateway.Application.UnitTests.Key.Commands
{
    public class CreateKeyCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ISnapshotService> _mockSnapshotService;
        private readonly Mock<ILogger<CreateKeyCommandHandler>> _mockLogger;
        private readonly Mock<IKeyRepository> _mockKeyRepository;
        private readonly Mock<IKeyService> _mockKeyService;
        private readonly Mock<IPolicyService> _mockPolicyService;

        public CreateKeyCommandHandlerTests()
        {
            _mockKeyRepository = KeyRepositoryMocks.GetKeyRepository();
            _mockKeyService = KeyServiceMocks.GetKeyService();
            _mockPolicyService = PolicyServiceMocks.GetPolicyService();
            _mockSnapshotService = new Mock<ISnapshotService>();
            _mockLogger = new Mock<ILogger<CreateKeyCommandHandler>>();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_create_Key()
        {
            var handler = new CreateKeyCommandHandler(_mockKeyRepository.Object, _mockKeyService.Object, _mapper, _mockLogger.Object, _mockSnapshotService.Object, _mockPolicyService.Object);
            var result = await handler.Handle(new CreateKeyCommand()
            {
                KeyName = "keyName",
                Rate = 10,
                Per = 10,
                Quota = 10,
                QuotaRenewalRate = 10,
                ThrottleInterval = 10,
                ThrottleRetries = 10,
                Expires = 10,           
                AccessRights = new List<KeyAccessRightsModel>
                            {
                                new KeyAccessRightsModel()
                                    { ApiId= Guid.Parse("{e07c7e99-3f5f-4ca3-b377-5b15eecbb0ca}"),
                                      ApiName="apiName",
                                      Versions = new List<string> { "version1","version2"},
                                      AllowedUrls = new List<KeyAllowedUrl>{new KeyAllowedUrl() { Url = "url", Methods = new List<string> { "method1", "method2" } } },
                                      Limit = new KeyApiLimit(){Rate=10,Per=10,Throttle_interval=10,Throttle_retry_limit=10,Max_query_depth=10,Quota_max=10,Quota_renews = 10,Quota_remaining =10,Quota_renewal_rate=10}
                                     }
                            },
                Policies = new List<string> { "EE272F8B-6096-4CB6-8625-BB4BB2D89E8B", "7cca2947-221d-4314-971e-911d542622b2" }
            },
            CancellationToken.None);
            var Keys = await _mockKeyRepository.Object.ListAllAsync();
            result.ShouldBeOfType<Response<Domain.GatewayCommon.Key>>();
            result.Succeeded.ShouldBeTrue();
            Keys.Count.ShouldBe(3);
        }
    }
}
