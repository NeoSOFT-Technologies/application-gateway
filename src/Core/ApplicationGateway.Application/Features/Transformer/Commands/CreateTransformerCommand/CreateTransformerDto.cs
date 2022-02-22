using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Transformer.Commands.CreateTransformerCommand
{
    public class CreateTransformerDto
    {
        public Guid Id { get; set; }
        public string TemplateName { get; set; }
    }
}
