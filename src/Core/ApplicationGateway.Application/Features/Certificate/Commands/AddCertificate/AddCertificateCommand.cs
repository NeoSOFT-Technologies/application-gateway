using MediatR;
using Microsoft.AspNetCore.Http;

namespace ApplicationGateway.Application.Features.Certificate.Commands.AddCertificate
{
    public class AddCertificateCommand:IRequest<Guid>
    {
        public IFormFile File { get; set; }
    }
}
