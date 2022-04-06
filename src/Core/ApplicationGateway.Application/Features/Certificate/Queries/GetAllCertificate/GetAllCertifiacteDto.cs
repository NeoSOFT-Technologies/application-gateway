using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Certificate.Queries.GetAllCertificate
{
    public class GetAllCertifiacteDto
    {
        public List<Certificate> CertificateCollection { get; set; }
    }
    public class Certificate
    {
        public Guid CertId { get; set; }
        public string CommanName { get; set; }
        public string Email { get; set; }
    }
}
