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
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
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
            mockMediator.Setup(m => m.Send(It.IsAny<GetAllApisQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetAllApisDto>());
            mockMediator.Setup(m => m.Send(It.IsAny<GetApiByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetApiByIdDto>());


            mockMediator.Setup(m => m.Send(It.IsAny<CreateApiCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<CreateApiDto>());
            mockMediator.Setup(m => m.Send(It.IsAny<CreateMultipleApisCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<CreateMultipleApisDto>());
           
            mockMediator.Setup(m => m.Send(It.IsAny<DeleteApiCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Unit());

            mockMediator.Setup(m => m.Send(It.IsAny<UpdateApiCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<UpdateApiDto>());

            //key
            mockMediator.Setup(m => m.Send(It.IsAny<GetAllKeysQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetAllKeysDto>());
            mockMediator.Setup(m => m.Send(It.IsAny<GetKeyQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<Key>());

            mockMediator.Setup(m => m.Send(It.IsAny<CreateKeyCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<Key>());
            mockMediator.Setup(m => m.Send(It.IsAny<DeleteKeyCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Unit());
            mockMediator.Setup(m => m.Send(It.IsAny<UpdateKeyCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<UpdateKeyCommandDto>());





            return mockMediator;



        }
    }
}
