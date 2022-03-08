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

namespace ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand
{
    public class UpdateApiCommandHandler : IRequestHandler<UpdateApiCommand, Response<UpdateApiDto>>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateApiCommandHandler> _logger;
        private readonly IApiDtoRepository _apiDtoRepository;
        public UpdateApiCommandHandler(ISnapshotService snapshotService, IApiService apiService, IApiDtoRepository apiDtoRepository, IMapper mapper, ILogger<UpdateApiCommandHandler> logger)
        {
            _snapshotService = snapshotService;
            _apiDtoRepository = apiDtoRepository;
            _apiService = apiService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<UpdateApiDto>> Handle(UpdateApiCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@UpdateApiCommand}", request);

            #region Check if API exists
            await _apiService.GetApiByIdAsync(request.ApiId);
            #endregion

            Domain.Entities.Api apiToUpdate = _mapper.Map<Domain.Entities.Api>(request);

            if (!await _apiService.CheckUniqueListenPathAsync(apiToUpdate))
            {
                throw new BadRequestException("ListenPath already exists");
            }

            Domain.Entities.Api updatedApi = await _apiService.UpdateApiAsync(apiToUpdate);

            UpdateApiDto updateApiDto = _mapper.Map<UpdateApiDto>(updatedApi);

            #region CreateSnapshot
            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.API,
                Enums.Operation.Updated,
                request.ApiId.ToString(),
                updatedApi);
            #endregion

            #region Update to Api Dto
            ApiDto apiDto = new ApiDto()
            {
                Id=updatedApi.ApiId,
                Name=updatedApi.Name,
                TargetUrl=updatedApi.TargetUrl,
                IsActive=true,
                Version=""
            };
            await _apiDtoRepository.UpdateAsync(apiDto);
            #endregion

            Response<UpdateApiDto> result = new Response<UpdateApiDto>(updateApiDto, "success");

            _logger.LogInformation("Handler Completed");
            return result;
        }
    }
}