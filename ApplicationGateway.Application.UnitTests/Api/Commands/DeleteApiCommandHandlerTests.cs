using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Api.Commands.DeleteApiCommand;
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
    public class DeleteApiCommandHandlerTests
    {
        private readonly Mock<ISnapshotService> _snapshotService;
        private readonly Mock<ILogger<DeleteApiCommandHandler>> _mockLogger;
        private readonly Mock<IApiRepository> _mockApiRepository;
        private readonly Mock<IApiService> _mockApiService;

        public DeleteApiCommandHandlerTests()
        {
            _mockApiRepository = ApiRepositoryMocks.GetApiRepository();
            _mockApiService = ApiServiceMocks.GetApiService();
            _snapshotService = new Mock<ISnapshotService>();
            _mockLogger = new Mock<ILogger<DeleteApiCommandHandler>>();

        }

        [Fact]
        public async Task Handle_Deleted_From_ApiRepo()
        {
            var ApiId = _mockApiService.Object.GetAllApisAsync().Result.FirstOrDefault().ApiId;
            var oldApi = await _mockApiService.Object.GetApiByIdAsync(ApiId);
            var handler = new DeleteApiCommandHandler(_mockApiRepository.Object,_snapshotService.Object, _mockApiService.Object, _mockLogger.Object);
            var result = await handler.Handle(new DeleteApiCommand() { ApiId= ApiId},CancellationToken.None);
            var allApis = await _mockApiService.Object.GetAllApisAsync();
            allApis.ShouldNotContain(oldApi);
            allApis.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_Api_Not_Found()
        {
            var ApiID = Guid.Parse("{85a8c9cb-563d-4a3a-b101-1dab23a51a6d}");
            var handler = new DeleteApiCommandHandler(_mockApiRepository.Object, _snapshotService.Object, _mockApiService.Object, _mockLogger.Object);
            var result = await handler.Handle(new DeleteApiCommand() { ApiId = ApiID }, CancellationToken.None); 
            var allApis = await _mockApiService.Object.GetAllApisAsync();
            allApis.Count.ShouldBe(2);           
        }
    }
}
