using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Policy.Commands.UpdatePolicyCommand;
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
    public class UpdatePolicyCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ISnapshotService> _snapshotService;
        private readonly Mock<ILogger<UpdatePolicyCommandHandler>> _mockLogger;
        private readonly Mock<IPolicyRepository> _mockPolicyRepository;
        private readonly Mock<IPolicyService> _mockPolicyService;

        public UpdatePolicyCommandHandlerTests()
        {
            _mockPolicyRepository = PolicyRepositoryMocks.GetPolicyRepository();
            _mockPolicyService = PolicyServiceMocks.GetPolicyService();
            _snapshotService = new Mock<ISnapshotService>();
            _mockLogger = new Mock<ILogger<UpdatePolicyCommandHandler>>();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_Update_Policy()
        {
            var policyId = _mockPolicyService.Object.GetAllPoliciesAsync().Result.FirstOrDefault().PolicyId;
            var handler = new UpdatePolicyCommandHandler(_mockPolicyRepository.Object, _mockPolicyService.Object, _mapper,  _mockLogger.Object, _snapshotService.Object);
            var oldPolicy = await _mockPolicyService.Object.GetPolicyByIdAsync(policyId);
            var result = await handler.Handle(new UpdatePolicyCommand()
            {
                PolicyId = policyId,
                Name = "UpdatedPolicy",
                Active = true,
                KeysInactive = true,
                MaxQuota = 5,
                QuotaRate = 5,
                Rate = 3,
                Per = 20,
                ThrottleInterval = 10,
                ThrottleRetries = 10,
                State = "UpdatedState",
                KeyExpiresIn = 10,
                Tags = new List<string> { "tag3", "tag4" },
                APIs = new List<UpdatePolicyApi>
                             { new UpdatePolicyApi()
                                { Id = Guid.Parse("{7cca2947-221d-4314-971e-911d542622b2}"),
                                 Name = "policyApiName",
                                 Versions = new List<string> { "version3", "version4" },
                                 AllowedUrls = new List<UpdateAllowedUrl>{new UpdateAllowedUrl() { url = "url", methods = new List<string> { "method1", "method2" } } },
                                 Limit=new UpdatePerApiLimit() { rate=10,per=10,throttle_interval=10,throttle_retry_limit=10,max_query_depth=10,quota_max=10,quota_renews = 10,quota_remaining =10,quota_renewal_rate=10,set_by_policy=true} } },
                Partitions = new UpdatePartition() { quota = true, rate_limit = true, complexity = true, acl = true, per_api = true },
            },
            CancellationToken.None);
            var allPolicies = await _mockPolicyService.Object.GetAllPoliciesAsync();
            allPolicies.Count.ShouldBe(2);
            allPolicies[0].Name.ShouldBeEquivalentTo("UpdatedPolicy");
            allPolicies[0].State.ShouldBeEquivalentTo("UpdatedState");
            result.ShouldNotBeNull();


        }
    }
}
