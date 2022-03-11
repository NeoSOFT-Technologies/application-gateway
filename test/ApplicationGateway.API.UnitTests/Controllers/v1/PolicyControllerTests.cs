using ApplicationGateway.Api.Controllers;
using ApplicationGateway.API.UnitTests.Mocks;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Application.Features.Policy.Commands.UpdatePolicyCommand;
using ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery;
using ApplicationGateway.Application.Features.Policy.Queries.GetPolicyByIdQuery;
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
    public class PolicyControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<PolicyController>> _mockLogger;

        public PolicyControllerTests()
        {
            _mockLogger = new Mock<ILogger<PolicyController>>();
            _mockMediator = MediatorMocks.GetMediator();
        }

        [Fact]
        public async Task Get_PolicyList()
        {
            var controller = new PolicyController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.GetAllPolicies(1,1);

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<PagedResponse<GetAllPoliciesDto>>();
        }

        [Fact]
        public async Task Get_PolicyById()
        {
            var controller = new PolicyController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.GetPolicyByid(new Guid());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<GetPolicyByIdDto>>();
        }

        [Fact]
        public async Task Create_Policy()
        {
            var controller = new PolicyController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.CreatePolicy(new CreatePolicyCommand());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<CreatePolicyDto>>();
        }

        [Fact]
        public async Task Delete_Policy()
        {
            var controller = new PolicyController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.DeletePolicy(new Guid());

            result.ShouldBeOfType<NoContentResult>();
            var noContentResult = result as NoContentResult;
            noContentResult.StatusCode.ShouldBe(204);

        }

        [Fact]
        public async Task Update_Policy()
        {
            var controller = new PolicyController(_mockMediator.Object, _mockLogger.Object);

            var result = await controller.UpdatePolicy(new UpdatePolicyCommand());

            result.ShouldBeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.StatusCode.ShouldBe(200);
            okObjectResult.Value.ShouldNotBeNull();
            okObjectResult.Value.ShouldBeOfType<Response<UpdatePolicyDto>>();

        }
    }
}
