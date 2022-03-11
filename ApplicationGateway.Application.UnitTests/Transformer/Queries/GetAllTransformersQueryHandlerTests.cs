using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Transformers.Queries.GetAllTransformer;
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

namespace ApplicationGateway.Application.UnitTests.Transformer.Queries
{
    public class GetAllTransformersQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<GetAllTransformersQueryHandler>> _mocklogger;
        private readonly Mock<ITransformerRepository> _mockTransformerRepository;

        public GetAllTransformersQueryHandlerTests()
        {
            _mockTransformerRepository = TransformerRepositoryMocks.GetTransformerRepository();
            _mocklogger = new Mock<ILogger<GetAllTransformersQueryHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_GetAllTransformers()
        {
            var handler = new GetAllTransformersQueryHandler(_mapper, _mocklogger.Object, _mockTransformerRepository.Object);

            var result = await handler.Handle(new GetAllTransformersQuery(), CancellationToken.None);
            var allKeys = await _mockTransformerRepository.Object.ListAllAsync();
            result.ShouldBeOfType<Response<IEnumerable<GetAllTransformersDto>>>();
            allKeys.Count.ShouldBe(2);



        }
    }
}
