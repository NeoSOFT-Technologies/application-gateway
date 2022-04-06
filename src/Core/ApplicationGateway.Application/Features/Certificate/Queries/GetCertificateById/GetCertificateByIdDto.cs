using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Certificate.Queries.GetCertificateById
{
    public class GetCertificateByIdDto
    {
        public Guid CertId { get; set; }
        public string Subject{ get; set; }
        public string Issuer{ get; set; }
        public DateTime ValidNotBefore { get; set; }
        public DateTime ValidNotAfter { get; set; }
        public string SignatureAlgorithm { get; set; }
        public string Thumbprint { get; set; }
    }
}
