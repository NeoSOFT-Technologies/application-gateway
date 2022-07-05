using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Policy.Commands.DeletePolicyCommand;
using ApplicationGateway.Application.Profiles;
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
    public class DeletePolicyCommandHandlerTests
    {
        private readonly Mock<ISnapshotService> _snapshotService;
        private readonly Mock<ILogger<DeletePolicyCommandHandler>> _mockLogger;
        private readonly Mock<IPolicyRepository> _mockPolicyRepository;
        private readonly Mock<IPolicyService> _mockPolicyService;
        private readonly Mock<IKeyRepository> _mockKeyRepository;
        private readonly Mock<IKeyService> _mockKeyService;
        private readonly IMapper _mapper;

        public DeletePolicyCommandHandlerTests()
        {
            _mockPolicyRepository = PolicyRepositoryMocks.GetPolicyRepository();
            _mockPolicyService = PolicyServiceMocks.GetPolicyService();
            _mockKeyRepository = KeyRepositoryMocks.GetKeyRepository();
            _mockKeyService = KeyServiceMocks.GetKeyService();
            _snapshotService = new Mock<ISnapshotService>();
            _mockLogger = new Mock<ILogger<DeletePolicyCommandHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]

        public async Task Handle_Delete_From_PolicyRepo()
        {
            var PolicyId = _mockPolicyService.Object.GetAllPoliciesAsync().Result.FirstOrDefault().PolicyId;
            var oldPolicy = await _mockPolicyService.Object.GetPolicyByIdAsync(PolicyId);
            var handler = new DeletePolicyCommandHandler(_mockPolicyRepository.Object, _mockPolicyService.Object, _mockKeyRepository.Object, _mockKeyService.Object, _mapper, _mockLogger.Object, _snapshotService.Object);
            var result = await handler.Handle(new DeletePolicyCommand() { PolicyId = PolicyId }, CancellationToken.None);
            var allPolicies = await _mockPolicyService.Object.GetAllPoliciesAsync();
            allPolicies.ShouldNotContain(oldPolicy);
            allPolicies.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_Policy_Not_Found()
        {
            var PolicyId = Guid.Parse("{85a8c9cb-563d-4a3a-b101-1dab23a51a6d}");
            var handler = new DeletePolicyCommandHandler(_mockPolicyRepository.Object, _mockPolicyService.Object, _mockKeyRepository.Object, _mockKeyService.Object, _mapper, _mockLogger.Object, _snapshotService.Object);
            var result = await handler.Handle(new DeletePolicyCommand() { PolicyId = PolicyId }, CancellationToken.None);
            var allPolicies = await _mockPolicyService.Object.GetAllPoliciesAsync();
            allPolicies.Count.ShouldBe(2);
        }
    }
}
