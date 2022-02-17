using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Helper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Api.Commands.DeleteApiCommand
{
    public class DeleteApiCommandHandler : IRequestHandler<DeleteApiCommand>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IApiService _apiService;
        private readonly ILogger<DeleteApiCommandHandler> _logger;

        public DeleteApiCommandHandler(ISnapshotService snapshotService, IApiService apiService, ILogger<DeleteApiCommandHandler> logger)
        {
            _snapshotService = snapshotService;
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteApiCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@DeleteApiCommand}", request);
            Guid apiId = request.ApiId;

            #region Check if API exists
            await _apiService.GetApiByIdAsync(request.ApiId);
            #endregion

            await _apiService.DeleteApiAsync(apiId);

            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.API,
                Enums.Operation.Deleted,
                request.ApiId.ToString(),
                null);

            _logger.LogInformation("Handler Completed");
            return Unit.Value;
        }
    }
}