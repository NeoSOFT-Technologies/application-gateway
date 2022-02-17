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

            #region Get All Existsing APIs
            List<Domain.Entities.Api> allApis = await _apiService.GetAllApisAsync();
            #endregion

            #region Add APIs one by one
            CreateMultipleApisDto createMultipleApisDto = new CreateMultipleApisDto() { APIs = new List<MultipleApiModelDto>() };
            foreach (MultipleApiModel obj in request.APIs)
            {
                #region Check for existing listenPath
                foreach (Domain.Entities.Api API in allApis)
                {
                    if (obj.ListenPath.Trim('/') == API.ListenPath.Trim('/'))
                    {
                        throw new BadRequestException("Listen path already exists");
                    }
                }
                #endregion

                Domain.Entities.Api api = _mapper.Map<Domain.Entities.Api>(obj);
                Domain.Entities.Api newApi = await _apiService.CreateApiAsync(api);

                MultipleApiModelDto multipleApiModelDto = _mapper.Map<MultipleApiModelDto>(newApi);
                createMultipleApisDto.APIs.Add(multipleApiModelDto);

                await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.API,
                Enums.Operation.Created,
                newApi.ApiId,
                newApi);
            }
            #endregion

            Response<CreateMultipleApisDto> response = new Response<CreateMultipleApisDto>(createMultipleApisDto, "success");

            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}