using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Key.Queries.GetAllKeys;
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

namespace ApplicationGateway.Application.UnitTests.Key.Queries
{
    public class GetAllKeysQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IKeyRepository> _mockKeyRepository;
        private readonly Mock<ILogger<GetAllKeysQueryHandler>> _mockLogger;

        public GetAllKeysQueryHandlerTests()
        {
            _mockKeyRepository = KeyRepositoryMocks.GetKeyRepository();
            _mockLogger = new Mock<ILogger<GetAllKeysQueryHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_GetAllKeys()
        {
            var handler = new GetAllKeysQueryHandler(_mockKeyRepository.Object,_mockLogger.Object, _mapper);

            var query = new GetAllKeysQuery();
            query.searchParam = new();
            query.sortParam = new();
            var result = await handler.Handle(query, CancellationToken.None);
            var allKeys = await _mockKeyRepository.Object.ListAllAsync();
            result.ShouldBeOfType<PagedResponse<GetAllKeysDto>>();
            allKeys.Count.ShouldBe(2);



        }
    }
}
