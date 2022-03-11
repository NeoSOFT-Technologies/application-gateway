using ApplicationGateway.Api.Controllers;
using ApplicationGateway.API.UnitTests.Mocks;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery;
using ApplicationGateway.Application.Features.Api.Queries.GetApiByIdQuery;
using ApplicationGateway.Application.Responses;
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
    public class ApplicationGatewayControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<ApplicationGatewayController>> _mockLogger;
        public ApplicationGatewayControllerTests()
        {
            _mockLogger = new Mock<ILogger<ApplicationGatewayController>>();
            _mockMediator = MediatorMocks.GetMediator();

        }
        [Fact]
        public async Task Get_ApiList()
        {
            var controller = new ApplicationGatewayController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.GetAllApis(1,1);

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<GetAllApisDto>>();
        }

        [Fact]
        public async Task Get_ApiById()
        {
            var controller = new ApplicationGatewayController(_mockMediator.Object,_mockLogger.Object);

            var result = await controller.GetApiById(Guid.NewGuid());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<GetApiByIdDto>>();
        }

        [Fact]
        public async Task Create_Api()
        {
            var controller = new ApplicationGatewayController(_mockMediator.Object,_mockLogger.Object);

            var result = await controller.CreateApi(new CreateApiCommand());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<CreateApiDto>>();
        }

        [Fact]
        public async Task Create_MultipleApi()
        {
            var controller = new ApplicationGatewayController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.CreateMultipleApis(new CreateMultipleApisCommand());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<CreateMultipleApisDto>>();

        }

        [Fact]
        public async Task Delete_Api()
        {
            var controller = new ApplicationGatewayController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.DeleteApi(Guid.NewGuid());

            result.ShouldBeOfType<NoContentResult>();
            var noContentResult = result as NoContentResult;
            noContentResult.StatusCode.ShouldBe(204);
        }

        [Fact]
        public async Task Update_Api()
        {
            var controller = new ApplicationGatewayController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.UpdateApi(new UpdateApiCommand());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<UpdateApiDto>>();
        }
    }
}
