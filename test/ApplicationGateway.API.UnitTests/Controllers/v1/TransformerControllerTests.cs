using ApplicationGateway.Api.Controllers.v1;
using ApplicationGateway.API.UnitTests.Mocks;
using ApplicationGateway.Application.Features.Transformers.Commands.CreateTransformerCommand;
using ApplicationGateway.Application.Features.Transformers.Commands.UpdateTransformerCommand;
using ApplicationGateway.Application.Features.Transformers.Queries.GetAllTransformer;
using ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerById;
using ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerByName;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationGateway.API.UnitTests.Controllers.v1
{
    public class TransformerControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<TransformerController>> _mockLogger;

        public TransformerControllerTests()
        {
            _mockLogger = new Mock<ILogger<TransformerController>>();
            _mockMediator = MediatorMocks.GetMediator();
        }

        [Fact]
        public async Task Get_TransformerList()
        {
            var controller = new TransformerController(_mockMediator.Object,_mockLogger.Object);

            var result = await controller.GetAllTransformers();

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<IEnumerable<GetAllTransformersDto>>>();
        }

        [Fact]
        public async Task Get_TransformerById()
        {
            var controller = new TransformerController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.GetTransformerById(new Guid());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<GetTransformerByIdDto>>();
        }

        //[Fact]
        //public async Task Get_TransformerByName()
        //{
        //    var controller = new TransformerController(_mockMediator.Object, _mockLogger.Object);

        //    var result = await controller.GetTransformerByName(new GetTransformerByNameQuery());

        //    result.ShouldBeOfType<OkObjectResult>();
        //    var okObjectResult = result as OkObjectResult;
        //    okObjectResult.StatusCode.ShouldBe(200);
        //    okObjectResult.Value.ShouldNotBeNull();
        //    okObjectResult.Value.ShouldBeOfType<Response<GetTransformerByIdDto>>();
        //}

        [Fact]
        public async Task Create_Transformer()
        {
            var controller = new TransformerController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.CreateTransformer(new CreateTransformerCommand());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<CreateTransformerDto>>();
        }

        [Fact]
        public async Task Delete_Transformer()
        {
            var controller = new TransformerController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.DeleteTransformer(new Guid());

            result.ShouldBeOfType<NoContentResult>();
            var noContentResult = result as NoContentResult;
            noContentResult.StatusCode.ShouldBe(204);
        }

        [Fact]
        public async Task Update_Transformer()
        {
            var controller = new TransformerController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.UpdateTransformer(new UpdateTransformerCommand());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<UpdateTransformerDto>>();
        }
    }
}
