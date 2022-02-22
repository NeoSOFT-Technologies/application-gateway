using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Responses;
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

        public UpdateApiCommandHandler(ISnapshotService snapshotService, IApiService apiService, IMapper mapper, ILogger<UpdateApiCommandHandler> logger)
        {
            _snapshotService = snapshotService;
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

            Domain.Entities.Api api = _mapper.Map<Domain.Entities.Api>(request);
            Domain.Entities.Api newApi = await _apiService.UpdateApiAsync(api);

            UpdateApiDto updateApiDto = _mapper.Map<UpdateApiDto>(newApi);

            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.API,
                Enums.Operation.Updated,
                request.ApiId.ToString(),
                newApi);

            Response<UpdateApiDto> result = new Response<UpdateApiDto>(updateApiDto, "success");

            _logger.LogInformation("Handler Completed");
            return result;
        }
    }
}