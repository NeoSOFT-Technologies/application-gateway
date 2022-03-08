using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand
{
    public class CreateApiCommandHandler : IRequestHandler<CreateApiCommand, Response<CreateApiDto>>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateApiCommandHandler> _logger;
        private readonly IApiDtoRepository _apiDtoRepository;

        public CreateApiCommandHandler(ISnapshotService snapshotService, IApiService apiService, IMapper mapper, ILogger<CreateApiCommandHandler> logger, IApiDtoRepository apiDtoRepository)
        {
            _apiDtoRepository = apiDtoRepository;
            _snapshotService = snapshotService;
            _apiService = apiService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<CreateApiDto>> Handle(CreateApiCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@CreateApiCommand}", request);
            Domain.Entities.Api apitoCreate = _mapper.Map<Domain.Entities.Api>(request);

            if (!await _apiService.CheckUniqueListenPathAsync(apitoCreate))
            {
                throw new BadRequestException("ListenPath already exists");
            }

            Domain.Entities.Api createdApi = await _apiService.CreateApiAsync(apitoCreate);

            CreateApiDto createApiDto = _mapper.Map<CreateApiDto>(createdApi);

            #region Create Snapshot
            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.API,
                Enums.Operation.Created,
                createdApi.ApiId.ToString(),
                createdApi);
            #endregion

            #region Create Api Dto
            ApiDto apiDto = new ApiDto()
            {
                Id = createdApi.ApiId,
                Name = createdApi.Name,
                TargetUrl = createdApi.TargetUrl,
                Version="",
                IsActive = true
            };
            await _apiDtoRepository.AddAsync(apiDto);
            #endregion

            Response<CreateApiDto> response = new Response<CreateApiDto>(createApiDto, "success");

            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}