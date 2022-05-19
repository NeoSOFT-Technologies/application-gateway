using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
using ApplicationGateway.Application.Features.Api.Commands.DeleteApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery;
using ApplicationGateway.Application.Features.Api.Queries.GetApiByIdQuery;
using ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand;
using ApplicationGateway.Application.Features.Key.Commands.DeleteKeyCommand;
using ApplicationGateway.Application.Features.Key.Commands.UpdateKeyCommand;
using ApplicationGateway.Application.Features.Key.Queries.GetAllKeys;
using ApplicationGateway.Application.Features.Key.Queries.GetKey;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Application.Features.Policy.Commands.DeletePolicyCommand;
using ApplicationGateway.Application.Features.Policy.Commands.UpdatePolicyCommand;
using ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery;
using ApplicationGateway.Application.Features.Policy.Queries.GetPolicyByIdQuery;
using ApplicationGateway.Application.Features.Transformers.Commands.CreateTransformerCommand;
using ApplicationGateway.Application.Features.Transformers.Commands.DeleteTransformerCommand;
using ApplicationGateway.Application.Features.Transformers.Commands.UpdateTransformerCommand;
using ApplicationGateway.Application.Features.Transformers.Queries.GetAllTransformer;
using ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerById;
using ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerByName;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.GatewayCommon;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationGateway.API.UnitTests.Mocks
{
    public class MediatorMocks
    {
        public static Mock<IMediator> GetMediator()
        {
            var mockMediator = new Mock<IMediator>();
            //api
            mockMediator.Setup(m => m.Send(It.IsAny<GetAllApisQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PagedResponse<GetAllApisDto>());
            mockMediator.Setup(m => m.Send(It.IsAny<GetApiByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetApiByIdDto>());


            mockMediator.Setup(m => m.Send(It.IsAny<CreateApiCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<CreateApiDto>());
            mockMediator.Setup(m => m.Send(It.IsAny<CreateMultipleApisCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<CreateMultipleApisDto>());
           
            mockMediator.Setup(m => m.Send(It.IsAny<DeleteApiCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Unit());

            mockMediator.Setup(m => m.Send(It.IsAny<UpdateApiCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<UpdateApiDto>());

            //key
            mockMediator.Setup(m => m.Send(It.IsAny<GetAllKeysQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PagedResponse<GetAllKeysDto>());
            mockMediator.Setup(m => m.Send(It.IsAny<GetKeyQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetKeyDto>());

            mockMediator.Setup(m => m.Send(It.IsAny<CreateKeyCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<Key>());
            mockMediator.Setup(m => m.Send(It.IsAny<DeleteKeyCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Unit());
            mockMediator.Setup(m => m.Send(It.IsAny<UpdateKeyCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<UpdateKeyCommandDto>());

            //policy
            mockMediator.Setup(m => m.Send(It.IsAny<GetAllPoliciesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PagedResponse<GetAllPoliciesDto>());
            mockMediator.Setup(m => m.Send(It.IsAny<GetPolicyByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetPolicyByIdDto>());

            mockMediator.Setup(m => m.Send(It.IsAny<CreatePolicyCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<CreatePolicyDto>());
            mockMediator.Setup(m => m.Send(It.IsAny<DeletePolicyCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Unit());

            mockMediator.Setup(m => m.Send(It.IsAny<UpdatePolicyCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<UpdatePolicyDto>());

            //transformer
            mockMediator.Setup(m => m.Send(It.IsAny<GetAllTransformersQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<IEnumerable<GetAllTransformersDto>>());
            mockMediator.Setup(m => m.Send(It.IsAny<GetTransformerByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetTransformerByIdDto>());
            mockMediator.Setup(m => m.Send(It.IsAny<GetTransformerByNameQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetTransformerByNameDto>());

            mockMediator.Setup(m => m.Send(It.IsAny<CreateTransformerCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<CreateTransformerDto>());
            mockMediator.Setup(m => m.Send(It.IsAny<DeleteTransformerCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Unit());
            mockMediator.Setup(m => m.Send(It.IsAny<UpdateTransformerCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<UpdateTransformerDto>());

            
            return mockMediator;

        }
    }
}
