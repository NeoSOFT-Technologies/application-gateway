using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Certificate.Commands.AddCertificate
{
    public class AddCertificateDto
    {
        public IFormFile FormFile { get; set; }
    }
}
