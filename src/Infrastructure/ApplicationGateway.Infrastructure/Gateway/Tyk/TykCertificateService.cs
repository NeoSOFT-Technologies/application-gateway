using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Models.Tyk;
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
            var filePath = $@"{certsPath}\{certId}.pem";

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

        public X509Certificate2 GetCertificateById(Guid certId)
        {
            _logger.LogInformation("GetCertificateById service initiated");

            //string certPath = $@"{_tykConfiguration.CertsPath}\{certId}.pfx";
            string certPath = $@"{_tykConfiguration.CertsPath}\{certId}.pem";
            if (!File.Exists(certPath))
                throw new FileNotFoundException();
            X509Certificate2Collection collection = new();
            //collection.Import(certPath, certPass, X509KeyStorageFlags.PersistKeySet);
            collection.ImportFromPemFile(certPath);

            _logger.LogInformation("GetCertificateById service commpleted");
            return collection.FirstOrDefault();
        }

        public X509Certificate2Collection GetAllCertificates()
        {
            _logger.LogInformation("GetAllCertificates service initiated");
            var certPathCollection = Directory.GetFiles(_tykConfiguration.CertsPath);

            X509Certificate2Collection collection = new();
            foreach(var certPath in certPathCollection)
                collection.ImportFromPemFile(certPath);

            _logger.LogInformation("GetAllCertificates service completed");
            return collection;
        }
    }
}
