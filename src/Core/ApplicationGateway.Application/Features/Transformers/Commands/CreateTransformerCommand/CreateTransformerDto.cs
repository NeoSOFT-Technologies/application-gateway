using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Transformers.Commands.CreateTransformerCommand
{
    public class CreateTransformerDto
    {
        public Guid TransformerId { get; set; }
        public string TemplateName { get; set; }
    }
}
