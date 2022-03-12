using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Transformers.Commands.DeleteTransformerCommand;
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

namespace ApplicationGateway.Application.UnitTests.Transformer.Commands
{
    public class DeleteTransformerCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<DeleteTransformerCommandHandler>> _mocklogger;
        private readonly Mock<ITransformerRepository> _mockTransformerRepository;

        public DeleteTransformerCommandHandlerTests()
        {
            _mockTransformerRepository = TransformerRepositoryMocks.GetTransformerRepository();
            _mocklogger = new Mock<ILogger<DeleteTransformerCommandHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_Deleted_From_TransformerRepo()
        {
            var TransformerId = _mockTransformerRepository.Object.ListAllAsync().Result.FirstOrDefault().TransformerId;
            var oldTransformer = await _mockTransformerRepository.Object.GetByIdAsync(TransformerId);
            var handler = new DeleteTransformerCommandHandler(_mapper, _mocklogger.Object, _mockTransformerRepository.Object);
            var result = await handler.Handle(new DeleteTransformerCommand() { TransformerId = TransformerId }, CancellationToken.None);
            var allApis = await _mockTransformerRepository.Object.ListAllAsync();
            allApis.ShouldNotContain(oldTransformer);
            allApis.Count.ShouldBe(1);
        }
    }
}
