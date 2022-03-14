using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Transformers.Commands.CreateTransformerCommand;
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

namespace ApplicationGateway.Application.UnitTests.Transformer.Commands
{
    public class CreateTransformerCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<CreateTransformerCommandHandler>> _mocklogger;
        private readonly Mock<ITransformerRepository> _mockTransformerRepository;

        public CreateTransformerCommandHandlerTests()
        {
            _mockTransformerRepository = TransformerRepositoryMocks.GetTransformerRepository();
            _mocklogger = new Mock<ILogger<CreateTransformerCommandHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_create_Transformer()
        {
            var handler = new CreateTransformerCommandHandler(_mapper, _mocklogger.Object, _mockTransformerRepository.Object);
            var result = await handler.Handle(new CreateTransformerCommand()
            {
                TemplateName ="testCreateTemplate",
                TransformerTemplate ="testTransformerTemplate",               
            },CancellationToken.None);
            result.ShouldBeOfType<Response<CreateTransformerDto>>();
            var allTransformers = await _mockTransformerRepository.Object.ListAllAsync();
            allTransformers.Count.ShouldBe(3);
            result.ShouldNotBeNull();
        }
    }
}
