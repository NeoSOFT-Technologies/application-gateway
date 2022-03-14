using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Key.Commands.DeleteKeyCommand;
using ApplicationGateway.Application.UnitTests.Mocks;
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

namespace ApplicationGateway.Application.UnitTests.Key.Commands
{
    public class DeleteKeyCommandHandlerTests
    {
        private readonly Mock<ISnapshotService> _snapshotService;
        private readonly Mock<ILogger<DeleteKeyCommandHandler>> _mockLogger;
        private readonly Mock<IKeyRepository> _mockKeyRepository;
        private readonly Mock<IKeyService> _mockKeyService;

        public DeleteKeyCommandHandlerTests()
        {
            _mockKeyRepository = KeyRepositoryMocks.GetKeyRepository();
            _mockKeyService = KeyServiceMocks.GetKeyService();
            _snapshotService = new Mock<ISnapshotService>();
            _mockLogger = new Mock<ILogger<DeleteKeyCommandHandler>>();
        }

        [Fact]
        public async Task Handle_Deleted_From_KeyRepo()
        {
            var KeyId = _mockKeyRepository.Object.ListAllAsync().Result.FirstOrDefault().Id;
            var handler = new DeleteKeyCommandHandler(_mockKeyRepository.Object, _mockKeyService.Object, _mockLogger.Object, _snapshotService.Object);
            await handler.Handle(new DeleteKeyCommand() { KeyId = KeyId }, CancellationToken.None);           
            var allKeys = await _mockKeyRepository.Object.ListAllAsync();           
            allKeys.Count.ShouldBe(1);
        }
    }
}
