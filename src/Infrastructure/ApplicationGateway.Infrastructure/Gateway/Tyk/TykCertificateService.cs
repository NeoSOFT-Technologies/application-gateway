using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Domain.GatewayCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Infrastructure.Gateway.Tyk
{
    [ExcludeFromCodeCoverage]
    public class TykCertificateService:ICertificateService
    {
        readonly ILogger<TykCertificateService> _logger;
        private readonly TykConfiguration _tykConfiguration;

        public TykCertificateService(ILogger<TykCertificateService> logger, IOptions<TykConfiguration> tykConfiguration)
        {
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
        }

        public async Task<Guid> AddCertificate(IFormFile file)
        {
            _logger.LogInformation("AddCertificate service initiated");
            if (Path.GetExtension(file.FileName).ToLowerInvariant() != ".pem")
                throw new FormatException("Only .pem file format is allowed");
            string certsPath = _tykConfiguration.CertsPath;

            if (!Directory.Exists(certsPath))
                Directory.CreateDirectory(certsPath);
            var certId = Guid.NewGuid();
            var filePath = $@"{certsPath}/{certId}.pem";
            using (var stream = System.IO.File.Create(filePath))
                await file.CopyToAsync(stream);
            _logger.LogInformation("AddCertificate service completed");
            return certId;
        }

        public void DeleteCertificate(Guid certId)
        {
            _logger.LogInformation("DeleteCertificate service initiated");
            string certPath = $@"{_tykConfiguration.CertsPath}\{certId}.pem";
            if (!File.Exists(certPath))
                throw new FileNotFoundException();
            File.Delete(certPath);

            _logger.LogInformation("DeleteCertificate service completed");
        }

        public Certificate GetCertificateById(Guid certId)
        {
            _logger.LogInformation("GetCertificateById service initiated");
            string certPath = $@"{_tykConfiguration.CertsPath}/{certId}.pem";
            if (!File.Exists(certPath))
                throw new NotFoundException("Certificate", certId);
            var cert = new X509Certificate2(File.ReadAllBytes(certPath));
            var certificate = mapCert(cert,certId);
            _logger.LogInformation("GetCertificateById service commpleted");
            return certificate;
        }

        public List<Certificate> GetAllCertificates()
        {
            _logger.LogInformation("GetAllCertificates service initiated");
            var certPathCollection = Directory.GetFiles(_tykConfiguration.CertsPath);
            List<Certificate> certificateCollection = new();
            foreach (var certPath in certPathCollection)
            {                
                var cert = new X509Certificate2(File.ReadAllBytes(certPath));
                var certificate = mapCert(cert, Guid.Parse(Path.GetFileNameWithoutExtension(certPath)));
                certificateCollection.Add(certificate);
            }
            _logger.LogInformation("GetAllCertificates service completed");
            return certificateCollection;
        }

        public bool CheckIfCertificateExists(IFormFile file)
        {
            X509Certificate2 certificate;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                certificate = new X509Certificate2(ms.ToArray());
            }
            List<Domain.GatewayCommon.Certificate> allCert = GetAllCertificates();

            foreach(var cert in allCert)
            
                if (certificate.Thumbprint == cert.Thumbprint)
                    return true;
            return false;
        }

        Certificate mapCert(X509Certificate2 cert,Guid certId) 
        {
            Certificate certificate = new();
            certificate.CertId = certId;
            certificate.Issuer = cert.IssuerName.Name;
            certificate.Subject = cert.SubjectName.Name;
            certificate.ValidNotBefore = cert.NotBefore;
            certificate.ValidNotAfter = cert.NotAfter;
            certificate.SignatureAlgorithm = cert.SignatureAlgorithm.FriendlyName;
            certificate.Thumbprint = cert.Thumbprint;
            return certificate;
        }
    }
}
