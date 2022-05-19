using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Certificate.Commands.DeleteCertificate
{
    public class DeleteCertificateCommandHandler:IRequestHandler<DeleteCertificateCommand>
    {
        readonly ILogger<DeleteCertificateCommandHandler> _logger;
        readonly ICertificateService _certificateService;

        public DeleteCertificateCommandHandler(ILogger<DeleteCertificateCommandHandler> logger, ICertificateService certificateService)
        {
            _logger = logger;
            _certificateService = certificateService;
        }

        public async Task<Unit> Handle(DeleteCertificateCommand command,CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeleteCertificateCommandHandler initiated");
            #region Validate if certificate exists
            _certificateService.GetCertificateById(command.CertId);
            #endregion

            _certificateService.DeleteCertificate(command.CertId);
            _logger.LogInformation("DeleteCertificateCommandHandler completed");

            return Unit.Value;
        }
    }
}
