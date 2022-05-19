using ApplicationGateway.Domain.GatewayCommon;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Contracts.Infrastructure.Gateway
{
    public interface ICertificateService
    {
        Task<Guid> AddCertificate(IFormFile file);
        void DeleteCertificate(Guid certId);
        Certificate GetCertificateById(Guid certId);
        List<Certificate> GetAllCertificates();
        bool CheckIfCertificateExists(IFormFile file);
    }
}
