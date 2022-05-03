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

namespace ApplicationGateway.Application.Features.Certificate.Queries.GetAllCertificate
{
    public class GetAllCertificateQueryHandler:IRequestHandler<GetAllCertificateQuery, GetAllCertificateDto>
    {
        readonly ILogger<GetAllCertificateQueryHandler> _logger;
        readonly IMapper _mapper;
        readonly ICertificateService _certificateService;

        public GetAllCertificateQueryHandler(ILogger<GetAllCertificateQueryHandler> logger, IMapper mapper, ICertificateService certificateService)
        {
            _logger = logger;
            _mapper = mapper;
            _certificateService = certificateService;
        }

        public async Task<GetAllCertificateDto> Handle(GetAllCertificateQuery request,CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllCertificateQueryHandler initiated");
            List<Domain.GatewayCommon.Certificate> certColl = _certificateService.GetAllCertificates();
            List<CertificateDto> allCert = new();
            certColl.ForEach(cert => {allCert.Add(_mapper.Map<CertificateDto>(cert)); 
                
            });

            _logger.LogInformation("GetAllCertificateQueryHandler completed");
            return new GetAllCertificateDto() { CertificateCollection = allCert };
        }
    }
}
