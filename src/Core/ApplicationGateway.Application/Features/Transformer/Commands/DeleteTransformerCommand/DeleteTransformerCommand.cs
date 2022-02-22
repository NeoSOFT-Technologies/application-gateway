using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Transformer.Commands.DeleteTransformerCommand
{
    public class DeleteTransformerCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
