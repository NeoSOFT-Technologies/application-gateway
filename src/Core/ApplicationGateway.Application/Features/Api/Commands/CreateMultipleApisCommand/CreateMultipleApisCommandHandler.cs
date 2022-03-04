using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand
{
    public class CreateMultipleApisCommandHandler : IRequestHandler<CreateMultipleApisCommand, Response<CreateMultipleApisDto>>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateMultipleApisCommandHandler> _logger;

        public CreateMultipleApisCommandHandler(ISnapshotService snapshotService, IApiService apiService, IMapper mapper, ILogger<CreateMultipleApisCommandHandler> logger)
        {
            _snapshotService = snapshotService;
            _apiService = apiService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<CreateMultipleApisDto>> Handle(CreateMultipleApisCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@CreateMultipleApisCommand}", request);
            #region Check for repeated listen path in request
            if (request.APIs.DistinctBy(p => p.ListenPath.Trim('/')).Count() != request.APIs.Count())
            {
                throw new BadRequestException("Listen path should be unique");
            }
            #endregion

            CreateMultipleApisDto createMultipleApisDto = new CreateMultipleApisDto() { APIs = new List<MultipleApiModelDto>() };

            #region Check for existing listenPath
            foreach (MultipleApiModel obj in request.APIs)
            {
                Domain.Entities.Api apiToCreate = _mapper.Map<Domain.Entities.Api>(obj);
                if (!await _apiService.CheckUniqueListenPathAsync(apiToCreate))
                {
                    throw new BadRequestException("ListenPath already exists");
                }
            }
            #endregion

            #region Add APIs one by one
            foreach (MultipleApiModel obj in request.APIs)
            {
                Domain.Entities.Api apiToCreate = _mapper.Map<Domain.Entities.Api>(obj);
                Domain.Entities.Api createdApi = await _apiService.CreateApiAsync(apiToCreate);

                MultipleApiModelDto multipleApiModelDto = _mapper.Map<MultipleApiModelDto>(createdApi);
                createMultipleApisDto.APIs.Add(multipleApiModelDto);

                await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.API,
                Enums.Operation.Created,
                createdApi.ApiId.ToString(),
                createdApi);
            }
            #endregion

            Response<CreateMultipleApisDto> response = new Response<CreateMultipleApisDto>(createMultipleApisDto, "success");

            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}