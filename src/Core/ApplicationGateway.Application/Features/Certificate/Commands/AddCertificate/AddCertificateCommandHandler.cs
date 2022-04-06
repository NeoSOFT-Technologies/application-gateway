using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Models.Tyk;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Certificate.Commands.AddCertificate
{
    public class AddCertificateCommandHandler : IRequestHandler<AddCertificateCommand,Guid>
    {
        readonly ILogger<AddCertificateCommandHandler> _logger;
        readonly ICertificateService _certificateService;

        public AddCertificateCommandHandler(ILogger<AddCertificateCommandHandler> logger, ICertificateService certificateService)
        {
            _logger = logger;
            _certificateService = certificateService;
        }

        public async Task<Guid> Handle(AddCertificateCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("AddCertificateCommandHandler initiated");
            Guid certId = await _certificateService.AddCertificate(command.File);
            _logger.LogInformation("AddCertificateCommandHandler completed");
            return certId;
        }

    }
}
