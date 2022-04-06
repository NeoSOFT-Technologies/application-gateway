using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Certificate.Commands.DeleteCertificate
{
    public class DeleteCertificateCommand:IRequest
    {
        public Guid CertId { get; set; }
    }
}
