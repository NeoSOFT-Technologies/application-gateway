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
    public class GetAllCertificateQueryHandler:IRequestHandler<GetAllCertificateQuery, X509Certificate2Collection>
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

        public async Task<X509Certificate2Collection> Handle(GetAllCertificateQuery request,CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllCertificateQueryHandler initiated");
            X509Certificate2Collection certColl = _certificateService.GetAllCertificates();
            _logger.LogInformation("GetAllCertificateQueryHandler completed");
            return certColl;
        }
    }
}
