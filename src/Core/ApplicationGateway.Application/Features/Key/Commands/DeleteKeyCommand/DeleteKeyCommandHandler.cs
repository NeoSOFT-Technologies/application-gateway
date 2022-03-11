using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Key.Commands.DeleteKeyCommand
{
    public class DeleteKeyCommandHandler:IRequestHandler<DeleteKeyCommand>
    {
        readonly ISnapshotService _snapshotService;
        readonly IKeyService _keyService;
        readonly ILogger<DeleteKeyCommandHandler> _logger;
        readonly IKeyRepository _keyRepository;

        public DeleteKeyCommandHandler(IKeyRepository keyDtoRepository, IKeyService keyService, ILogger<DeleteKeyCommandHandler> logger, ISnapshotService snapshotService)
        {
            _keyRepository = keyDtoRepository;
            _keyService = keyService;
            _logger = logger;
            _snapshotService = snapshotService;
        }

        public async Task<Unit> Handle(DeleteKeyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteKeyCommandHandler initated for {request}");
            #region Check If Key Exists
            await _keyService.GetKeyAsync(request.KeyId);
            #endregion

            await _keyService.DeleteKeyAsync(request.KeyId.ToString());

            #region Create SnapShot
            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.Key,
                Enums.Operation.Deleted,
                request.KeyId,
                null);
            #endregion

            #region Delete Key Dto
            await _keyRepository.DeleteAsync(new Domain.Entities.Key() { Id = request.KeyId });
            #endregion

            _logger.LogInformation($"DeleteKeyCommandHandler completed for {request}");
            return Unit.Value;
        }

    }
}
