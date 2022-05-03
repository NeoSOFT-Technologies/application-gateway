using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Certificate.Queries.GetAllCertificate
{
    public class GetAllCertificateQuery:IRequest<GetAllCertificateDto>
    {
    }
}
