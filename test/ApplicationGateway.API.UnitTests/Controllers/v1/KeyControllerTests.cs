using ApplicationGateway.Api.Controllers.v1;
using ApplicationGateway.API.UnitTests.Mocks;
using ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand;
using ApplicationGateway.Application.Features.Key.Commands.DeleteKeyCommand;
using ApplicationGateway.Application.Features.Key.Commands.UpdateKeyCommand;
using ApplicationGateway.Application.Features.Key.Queries.GetAllKeys;
using ApplicationGateway.Application.Features.Key.Queries.GetKey;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.GatewayCommon;
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
    public class KeyControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<KeyController>> _mockLogger;

        public KeyControllerTests()
        {
            _mockLogger = new Mock<ILogger<KeyController>>();
            _mockMediator = MediatorMocks.GetMediator();
        }

        [Fact]
        public async Task Get_KeyList()
        {
            var controller = new KeyController(_mockLogger.Object,_mockMediator.Object);

            var result = await controller.GetAllKeys(1,1);

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<PagedResponse<GetAllKeysDto>>();
        }

        [Fact]
        public async Task Get_Key()
        {
            var controller = new KeyController(_mockLogger.Object, _mockMediator.Object);

            var result = await controller.GetKey("key");

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<Key>>();
        }

        [Fact]
        public async Task Create_Key()
        {
            var controller = new KeyController(_mockLogger.Object, _mockMediator.Object);

            var result = await controller.CreateKey(new CreateKeyCommand());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<Key>>();
        }

        [Fact]
        public async Task Delete_Key()
        {
            var controller = new KeyController(_mockLogger.Object, _mockMediator.Object);

            var result = await controller.DeleteKey(new Guid().ToString());

            result.ShouldBeOfType<NoContentResult>();
            var noContentResult = result as NoContentResult;
            noContentResult.StatusCode.ShouldBe(204);
        }

        [Fact]
        public async Task Update_Key()
        {
            var controller = new KeyController(_mockLogger.Object, _mockMediator.Object);

            var result = await controller.UpdateKey(new UpdateKeyCommand());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<UpdateKeyCommandDto>>();
        }


    }
}
