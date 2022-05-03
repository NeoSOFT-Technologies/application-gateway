using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Certificate.Queries.GetCertificateById
{
    public class GetCertificateByIdQueryHandler:IRequestHandler<GetCertificateByIdQuery, GetCertificateByIdDto>
    {
        readonly ILogger<GetCertificateByIdQueryHandler> _logger;
        readonly IMapper _mapper;
        readonly ICertificateService _certificateService;

        public GetCertificateByIdQueryHandler(ILogger<GetCertificateByIdQueryHandler> logger, IMapper mapper, ICertificateService certificateService)
        {
            _logger = logger;
            _mapper = mapper;
            _certificateService = certificateService;
        }

        public async Task<GetCertificateByIdDto> Handle(GetCertificateByIdQuery request,CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetCertificateByIdQueryHandler initiated");
            var cert = _certificateService.GetCertificateById(request.CertId);
            GetCertificateByIdDto dto = new();
            dto.CertId = request.CertId;
            dto.Issuer= cert.IssuerName.Name;
            dto.Subject = cert.SubjectName.Name;
            //dto.Email = cert.SubjectName.Name;
            dto.ValidNotBefore = cert.NotBefore;
            dto.ValidNotAfter = cert.NotAfter;
            dto.SignatureAlgorithm = cert.SignatureAlgorithm.FriendlyName;
            dto.Thumbprint = cert.Thumbprint;
            _logger.LogInformation("GetCertificateByIdQueryHandler completed");
            return dto;
        }
    }
}
