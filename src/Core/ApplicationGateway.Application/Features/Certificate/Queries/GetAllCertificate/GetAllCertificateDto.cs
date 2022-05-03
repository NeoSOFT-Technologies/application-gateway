using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Certificate.Queries.GetAllCertificate
{
    public class GetAllCertificateDto
    {
        public List<CertificateDto> CertificateCollection { get; set; }
    }
    public class CertificateDto
    {
        public Guid CertId { get; set; }
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public DateTime ValidNotBefore { get; set; }
        public DateTime ValidNotAfter { get; set; }
        public string SignatureAlgorithm { get; set; }
        public string Thumbprint { get; set; }
    }
}
