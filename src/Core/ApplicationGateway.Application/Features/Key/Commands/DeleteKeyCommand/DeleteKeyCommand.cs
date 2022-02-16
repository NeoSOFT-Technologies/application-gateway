using ApplicationGateway.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Commands.DeleteKeyCommand
{
    public class DeleteKeyCommand:IRequest
    {
        public string KeyId { get; set; }
    }
}
