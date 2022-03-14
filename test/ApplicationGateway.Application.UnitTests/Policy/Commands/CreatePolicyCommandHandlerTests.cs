using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
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

namespace ApplicationGateway.Application.UnitTests.Policy.Commands
{
    public class CreatePolicyCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ISnapshotService> _snapshotService;
        private readonly Mock<ILogger<CreatePolicyCommandHandler>> _mockLogger;
        private readonly Mock<IPolicyRepository> _mockPolicyRepository;
        private readonly Mock<IPolicyService> _mockPolicyService;

        public CreatePolicyCommandHandlerTests()
        {
            _mockPolicyRepository = PolicyRepositoryMocks.GetPolicyRepository();
            _mockPolicyService = PolicyServiceMocks.GetPolicyService();
            _snapshotService = new Mock<ISnapshotService>();
            _mockLogger = new Mock<ILogger<CreatePolicyCommandHandler>>();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_create_Policy()
        {
            var handler = new CreatePolicyCommandHandler(_mockPolicyRepository.Object, _snapshotService.Object, _mockPolicyService.Object, _mapper, _mockLogger.Object);
            var result = await handler.Handle(new CreatePolicyCommand()
            {            
                Name = "policy3",
                Active = true,
                KeysInactive = true,
                MaxQuota = 5,
                QuotaRate = 5,
                Rate = 3,
                Per = 20,
                ThrottleInterval = 10,
                ThrottleRetries = 10,
                State = "state2",
                KeyExpiresIn = 10,
                Tags = new List<string> { "tag3", "tag4" },
                APIs = new List<CreatePolicyApi>
                             { new CreatePolicyApi()
                                { Id = Guid.Parse("{7cca2947-221d-4314-971e-911d542622b2}"),
                                 Name = "policyApiName",
                                 Versions = new List<string> { "version3", "version4" },
                                 AllowedUrls = new List<CreateAllowedUrl>{new CreateAllowedUrl() { url = "url", methods = new List<string> { "method1", "method2" } } },
                                 Limit=new CreatePerApiLimit() { rate=10,per=10,throttle_interval=10,throttle_retry_limit=10,max_query_depth=10,quota_max=10,quota_renews = 10,quota_remaining =10,quota_renewal_rate=10,set_by_policy=true} } },
                Partitions = new CreatePartition() { quota = true, rate_limit = true, complexity = true, acl = true, per_api = true },
            },
            CancellationToken.None);         
            result.ShouldBeOfType<Response<CreatePolicyDto>>();
            result.ShouldNotBeNull(); 
            

        }
    }
}
