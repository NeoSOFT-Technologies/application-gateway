using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerById;
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
    public class GetTransformerByIdQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<GetTransformerByIdQueryHandler>> _mocklogger;
        private readonly Mock<ITransformerRepository> _mockTransformerRepository;

        public GetTransformerByIdQueryHandlerTests()
        {
            _mockTransformerRepository = TransformerRepositoryMocks.GetTransformerRepository();
            _mocklogger = new Mock<ILogger<GetTransformerByIdQueryHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_Get_Transformer_by_id()
        {
            var transformerId = _mockTransformerRepository.Object.ListAllAsync().Result.FirstOrDefault().TransformerId;

            var handler = new GetTransformerByIdQueryHandler(_mapper,_mocklogger.Object, _mockTransformerRepository.Object);

            var result = await handler.Handle(new GetTransformerByIdQuery() { TransformerId = transformerId }, CancellationToken.None);

            result.ShouldBeOfType<Response<GetTransformerByIdDto>>();
            


        }
    }
}
