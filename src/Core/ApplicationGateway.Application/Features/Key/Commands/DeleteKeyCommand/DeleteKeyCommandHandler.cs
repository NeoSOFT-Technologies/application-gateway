using ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Commands.DeleteKeyCommand
{
    public class DeleteKeyCommandHandler:IRequestHandler<DeleteKeyCommand>
    {
        readonly ISnapshotService _snapshotService;
        readonly IKeyService _keyService;
        readonly ILogger<DeleteKeyCommandHandler> _logger;

        public DeleteKeyCommandHandler(IKeyService keyService,  ILogger<DeleteKeyCommandHandler> logger)
        {
            _keyService = keyService;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKeyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteKeyCommandHandler initated for {request}");
            await _keyService.DeleteKeyAsync(request.KeyId.ToString());
            _logger.LogInformation($"DeleteKeyCommandHandler completed for {request}");
            return Unit.Value;
        }

    }
}
